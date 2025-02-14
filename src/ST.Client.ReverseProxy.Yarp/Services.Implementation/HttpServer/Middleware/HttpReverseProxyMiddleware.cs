// https://github.com/dotnetcore/FastGithub/blob/2.1.4/FastGithub.HttpServer/HttpReverseProxyMiddleware.cs

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.Application.Models;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Yarp.ReverseProxy.Forwarder;

namespace System.Application.Services.Implementation.HttpServer.Middleware;

/// <summary>
/// 反向代理中间件
/// </summary>
sealed class HttpReverseProxyMiddleware
{
    static readonly IDomainConfig defaultDomainConfig = new DomainConfig() { TlsSni = true, };

    readonly IHttpForwarder httpForwarder;
    readonly IReverseProxyHttpClientFactory httpClientFactory;
    readonly IReverseProxyConfig reverseProxyConfig;
    readonly ILogger<HttpReverseProxyMiddleware> logger;

    public HttpReverseProxyMiddleware(
        IHttpForwarder httpForwarder,
        IReverseProxyHttpClientFactory httpClientFactory,
        IReverseProxyConfig reverseProxyConfig,
        ILogger<HttpReverseProxyMiddleware> logger)
    {
        this.httpForwarder = httpForwarder;
        this.httpClientFactory = httpClientFactory;
        this.reverseProxyConfig = reverseProxyConfig;
        this.logger = logger;
    }

    /// <summary>
    /// 处理请求
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var host = context.Request.Host;
        if (TryGetDomainConfig(new Uri(context.Request.GetDisplayUrl()), out var domainConfig) == false)
        {
            await next(context);
        }
        else if (domainConfig.Response == null)
        {
            var scheme = context.Request.Scheme;
            if (IReverseProxyService.Instance.EnableHttpProxyToHttps && scheme == "http")
            {
                context.Response.Redirect("https" + context.Request.Host.Host + context.Request.RawUrl());
                return;
            }

            var destinationPrefix = GetDestinationPrefix(scheme, host, domainConfig.Destination);
            var httpClient = httpClientFactory.CreateHttpClient(host.Host, domainConfig);
            if (!string.IsNullOrEmpty(domainConfig.UserAgent))
            {
                var oldua = context.Request.Headers.UserAgent.ToString();
                var newUA = domainConfig.UserAgent.Replace("${origin}", oldua, StringComparison.OrdinalIgnoreCase);
                context.Request.Headers.UserAgent = new StringValues(newUA);
            }

            var error = await httpForwarder.SendAsync(context, destinationPrefix, httpClient, ForwarderRequestConfig.Empty, HttpTransformer.Empty);
            await HandleErrorAsync(context, error);
        }
        else
        {
            context.Response.StatusCode = (int)domainConfig.Response.StatusCode;
            context.Response.ContentType = domainConfig.Response.ContentType;
            if (domainConfig.Response.ContentValue != null)
            {
                await context.Response.WriteAsync(domainConfig.Response.ContentValue);
            }
        }
    }

    bool TryGetDomainConfig(Uri uri, [MaybeNullWhen(false)] out IDomainConfig domainConfig)
    {
        if (reverseProxyConfig.TryGetDomainConfig(uri.Host + uri.AbsolutePath, out domainConfig) == true)
        {
            return true;
        }

        // 未配置的域名，但仍然被解析到本机 IP 的域名
        if (OperatingSystem.IsWindows() && IsDomain(uri.Host))
        {
            logger.LogWarning(
                $"域名 {uri.Host} 可能已经被 DNS 污染，如果域名为本机域名，请解析为非回环 IP。");
            domainConfig = defaultDomainConfig;
            return true;
        }

        return false;
    }

    /// <summary>
    /// 是否为域名
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    static bool IsDomain(string host) => !IPAddress.TryParse(host, out _) && host.Contains('.');

    /// <summary>
    /// 获取目标前缀
    /// </summary>
    /// <param name="scheme"></param>
    /// <param name="host"></param>
    /// <param name="destination"></param>
    /// <returns></returns>
    string GetDestinationPrefix(string scheme, HostString host, Uri? destination)
    {
        var defaultValue = $"{scheme}://{host}/";
        if (destination == null)
        {
            return defaultValue;
        }

        var baseUri = new Uri(defaultValue);
        var result = new Uri(baseUri, destination).ToString();
        logger.LogInformation($"{defaultValue} => {result}");
        return result;
    }

    /// <summary>
    /// 处理错误信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="error"></param>
    /// <returns></returns>
    static async Task HandleErrorAsync(HttpContext context, ForwarderError error)
    {
        if (error == ForwarderError.None || context.Response.HasStarted)
        {
            return;
        }

        await context.Response.WriteAsJsonAsync(new
        {
            error = error.ToString(),
            message = context.GetForwarderErrorFeature()?.Exception?.Message,
        });
    }
}
