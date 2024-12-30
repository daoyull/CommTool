

## 简介

`WPF`  + `Blazor ` 实现的通信测试工具，支持JS脚本、脚本调试、内置vscode代码编辑器，编写脚本代码有智能提示

## 技术点

* WPF
* Blazor
* MVVM
* IOC
* Monaco

## 功能支持

* 串口
* Tcp客户端
* Tcp服务端
* Udp
* 定时发送
* 超长文本显示
* 快速接收不卡界面
* Js 脚本
* 脚本编辑
* 脚本编辑智能提示

## 脚本支持

使用 `ClearScript` 作为脚本引擎，支持.Net对象注入，脚本中调用.Net方法，VsCode调试

脚本使用说明

脚本示例在 `Comm.WPF\scripts\common `中

## 打包方法

运行 `publish.bat` 会把项目打包到同一文件夹