<!--

### 公告
1. 非简中语言将默认隐藏加速和脚本功能，仅能通过切换语言并重启程序的方式还原被隐藏的功能
2. 现已停止短信服务节约开销，后续会推出邮箱注册登录，对于已使用手机号的用户需要进行绑定第三方快速登录，否则注销后将无法再次登录
3. 自动更新目前仅 Windows 端可用，且由于下载渠道限速可能导致无法更新成功，推荐在官网链接的网盘或群文件中下载压缩包解压覆盖更新(应用商店版由商店更新不受此影响)
4. 目前的本地加速功能的实现方式，因需要信任证书，在 Android 上因系统限制，所以此功能已放弃继续开发，如仍想使用需要自行导入证书到系统目录，使用 adb 工具或 Magisk 之类的软件操作，未来会使用不需要证书的加速功能替换此功能
5. fde 版本需要安装 [ASP.NET Core 运行时 6.0.6 (x64) 与 .NET Core 运行时 6.0.6 (x64)](https://dotnet.microsoft.com/zh-cn/download/dotnet/6.0)

-->

### 版本亮点
1. 新增 Steam 云存档管理功能，可自行上传或删除 Steam 云存档
2. 库存游戏支持筛选支持 Steam 云存档的游戏
3. ASF 升级至 V5.2.6.3
4. .NET 运行时升级至 6.0.6，使用 fde 版本需要升级运行库
5. 库存游戏中解锁成就与挂时长支持 macOS 与 Linux
6. 在设置中可关闭托盘，关闭托盘后关闭主窗口即退出程序
7. 网络加速中本地反代使用 Yarp 库替换之前的实现，提升稳定性
8. 新增 DNS 驱动拦截模式(仅 Windows) 进行本地加速
9. 新增 广告功能，赞助用户将会有特殊的标识并且不会显示广告

### 修复问题
1. 修复 窗口在某些情况下最大化或最小化恢复时窗口大小会变化的问题
2. 修复 库存游戏 编辑 Steam 游戏封面时选择自定义图片失败的问题
3. 修复 某些图片显示会错乱的问题
4. 修复 Linux 上点击关于页面可能因字体引发闪退

<!--

~~仅发布桌面版~~
. 修复 MIUI Android 11 ~ 12 中绑定或换绑手机号页面闪退

### 已知问题
- 除 Windows 之外的平台此软件自动更新尚不可用
- Desktop 
	- macOS
		- [尚未公证](https://support.apple.com/zh-cn/guide/mac-help/mh40616/10.15/mac/10.15)，这会影响 macOS Catalina（版本 10.15）以上版本
	- Linux
		- 窗口弹出位置不正确
		- 鼠标指针浮动样式不正确
	- Windows
		- Windows 11 
			- 在 CPU 不受支持的 Win11 上无法启动，Windows 日志中显示 ```Failed to create CoreCLR, HRESULT: 0x80004005```
			- 仅 .NET 6.0 受此影响，在内部版本 22509 中修复，见 [issue](https://github.com/dotnet/core/issues/6733)
			- **解决方案：** 可尝试使用旧版本 例如 v2.3.0
		- Windows 7
			- 先决条件
				- 需要安装 Extended Security Update
			- 在不符合先决条件的情况下运行可能导致
				- 程序无法正常运行
					- **解决方案**
						- 使用 Windows Update 更新系统补丁
				- 运行程序时提示 计算机中丢失 api-ms-win-core-winrt-l1-1-0.dll
					- **解决方案**
						- 下载 api-ms-win-core-winrt-l1-1-0.dll 文件放入程序根目录(Steam++.exe 所在文件夹)
							- [从 Github 上直接下载](https://github.com/BeyondDimension/SteamTools/raw/develop/references/runtime.win7-x64.Microsoft.NETCore.Windows.ApiSets/api-ms-win-core-winrt-l1-1-0.dll)
							- [从 Gitee 上直接下载](https://gitee.com/rmbgame/SteamTools/raw/develop/references/runtime.win7-x64.Microsoft.NETCore.Windows.ApiSets/api-ms-win-core-winrt-l1-1-0.dll)
	- Android
		- 本地加速
			- 因 Android 7(Nougat API 24) 之后的版本不在信任用户证书，所以此功能已放弃继续开发，如仍想使用需要自行导入证书到系统目录，使用 adb 工具或 Magisk 之类的软件操作，未来会使用不需要证书的加速功能替换此功能

-->


[![steampp.net](https://img.shields.io/badge/WebSite-steampp.net-brightgreen.svg?style=flat-square&color=61dafb)](https://steampp.net)
[![Watt Toolkit v2.8.0](https://img.shields.io/badge/Watt%20Toolkit-v2.8.0-brightgreen.svg?style=flat-square&color=512bd4)]()
  
  
##### [不知道该下载哪个文件?](./download-guide.md)
---

### 文件校验
|  File  | Checksum (SHA256)  |
|  ----  |  ----  |
| Steam++_win_x64_v2.7.2.7z  | SHA256 |
| Steam++_win_x64_fde_v2.7.2.7z  | SHA256 |
| | |
| Steam++_win_x64_v2.7.2.exe  | SHA256 |
| Steam++_win_x64_fde_v2.7.2.exe  | SHA256 |
| | |
| Steam++_linux_x64_v2.7.2.7z  | SHA256 |
| Steam++_linux_arm64_v2.7.2.7z  | SHA256 |
| | |
| Steam++_linux_x64_v2.7.2.deb  | SHA256 |
| Steam++_linux_arm64_v2.7.2.deb  | SHA256 |
| | |
| Steam++_linux_x64_v2.7.2.rpm  | SHA256 |
| Steam++_linux_arm64_v2.7.2.rpm  | SHA256 |
| | |
| Steam++_macos_x64_v2.7.2.dmg  | SHA256 |
| Steam++_macos_arm64_v2.7.2.dmg  | SHA256 |
| | |
| Steam++_android_v2.7.2.apk  | SHA256 |
