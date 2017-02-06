# Veldrid TinyDemo

This is a minimal, one-file demo program for [Veldrid](https://github.com/mellinoe/veldrid).

# Building
```
git clone --recursive https://github.com/mellinoe/veldrid.tinydemo
cd veldrid.tinydemo
dotnet restore src
dotnet restore ext/veldrid/src
dotnet msbuild
```

# Running
`bin/<OS>.x64.Debug/TinyDemo/TinyDemo<.exe> [opengl]`

Direct3D11 will be used by default on Windows, unless overridden by the passing "opengl" to the program. OpenGL is always used on Linux and macOS.