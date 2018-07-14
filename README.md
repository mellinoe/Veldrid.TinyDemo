NOTE: This repository contains example code for a legacy version of Veldrid (https://github.com/mellinoe/Veldrid-Legacy/), and is not compatible with the current version. Sample projects for the current version of Veldrid can be found in the [Veldrid Samples](https://github.com/mellinoe/veldrid-samples) repository.

# Veldrid TinyDemo

This is a minimal, one-file demo program for [Veldrid](https://github.com/mellinoe/veldrid).

# Requirements
This project uses the new MSBuild-based tooling for .NET Core. Please download the tools at https://github.com/dotnet/cli.

# Building
```
git clone --recursive https://github.com/mellinoe/veldrid.tinydemo
cd veldrid.tinydemo
dotnet restore src
dotnet restore ext/veldrid/src
dotnet msbuild src/TinyDemo/TinyDemo.csproj
```

# Running
`TinyDemo<.exe> [opengl]`

Direct3D11 will be used by default on Windows, unless overridden by the passing "opengl" to the program. OpenGL is always used on Linux and macOS.
