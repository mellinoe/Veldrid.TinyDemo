# Veldrid TinyDemo

This is a minimal, one-file demo program for [Veldrid](https://github.com/mellinoe/veldrid).

# Requirements
This project uses the new MSBuild-based tooling for .NET Core. Please download the tools at 

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