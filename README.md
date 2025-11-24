# LoveForm

<img width="3840" height="2080" alt="image" src="https://github.com/user-attachments/assets/2a315c49-507e-4ada-9b47-2ba75b973a6a" />

- 一个 Windows 平台的`夸夸程序`，目标框架 `net48`，语言版本 `C# 7.3`
- 支持发布为“单片 exe”（通过 `Costura.Fody` 将托管依赖嵌入可执行文件）
- GitHub Actions 基于标签自动构建、打包并发布 Release

## 下载
- 前往 Releases 页面下载最新版本：
  - https://github.com/Chendaqian/LoveForm/releases
- 资产说明：
  - `LoveFormsApp-<tag>.zip`：仅包含单片 `LoveForm.exe`
  - 需要在目标机器安装 `.NET Framework 4.8`

## 快速运行
- 解压后，直接双击 `LoveForm.exe`

## 系统要求
- .NET Framework：4.8（必须）