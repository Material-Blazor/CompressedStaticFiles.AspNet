<p align="center">
  <a href="https://github.com/AnderssonPeter/CompressedStaticFiles">
    <img src="icon.svg" alt="Logo" width="80" height="80">
  </a>

  <h3 align="center">CompressedStaticFiles</h3>

  <p align="center">
    Send compressed static files to the browser without having to compress on demand, also has support for sending more advanced image formats when the browser has support for it.
    <br />
    <br />
  </p>
</p>
<br />

[![NuGet version](https://img.shields.io/nuget/v/CompressedStaticFiles.AspNetCore?logo=nuget&label=nuget%20version&style=flat-square)](https://www.nuget.org/packages/CompressedStaticFiles.AspNetCore/)
[![NuGet version](https://img.shields.io/nuget/vpre/CompressedStaticFiles.AspNetCore?logo=nuget&label=nuget%20pre-release&style=flat-square)](https://www.nuget.org/packages/CompressedStaticFiles.AspNetCore/)
[![NuGet downloads](https://img.shields.io/nuget/dt/CompressedStaticFiles.AspNetCore?logo=nuget&label=nuget%20downloads&style=flat-square)](https://www.nuget.org/packages/CompressedStaticFiles.AspNetCore/)


---


[![GitHub license](https://img.shields.io/badge/license-Apache%202-blue.svg)](https://raw.githubusercontent.com/material-blazor/CompressedStaticFiles.AspNetCore/main/LICENSE)
[![GitHub issues](https://img.shields.io/github/issues/Material-Blazor/CompressedStaticFiles.AspNetCore?logo=github&style=flat-square)](https://github.com/Material-Blazor/CompressedStaticFiles.AspNetCore/issues)
[![GitHub forks](https://img.shields.io/github/forks/Material-Blazor/CompressedStaticFiles.AspNetCore?logo=github&style=flat-square)](https://github.com/Material-Blazor/CompressedStaticFiles.AspNetCore/network/members)
[![GitHub stars](https://img.shields.io/github/stars/Material-Blazor/CompressedStaticFiles.AspNetCore?logo=github&style=flat-square)](https://github.com/Material-Blazor/CompressedStaticFiles.AspNetCore/stargazers)
[![GitHub stars](https://img.shields.io/github/watchers/Material-Blazor/CompressedStaticFiles.AspNetCore?logo=github&style=flat-square)](https://github.com/Material-Blazor/CompressedStaticFiles.AspNetCore/watchers)

---

[![GithubActionsMainPublish](https://img.shields.io/github/workflow/status/Material-Blazor/CompressedStaticFiles.AspNetCore/GithubActionsRelease?label=actions%20release&logo=github&style=flat-square)](https://github.com/Material-Blazor/CompressedStaticFiles.AspNetCore/actions?query=workflow%3AGithubActionsRelease)
[![GithubActionsDevelop](https://img.shields.io/github/workflow/status/Material-Blazor/CompressedStaticFiles.AspNetCore/GithubActionsWIP?label=actions%20wip&logo=github&style=flat-square)](https://github.com/Material-Blazor/CompressedStaticFiles.AspNetCore/actions?query=workflow%3AGithubActionsWIP)

---




## Table of Contents
* [About the Project](#about-the-project)
* [Getting Started](#getting-started)
* [Example](#example)
* [Acknowledgements](#acknowledgements)

## About The Project
This project allows you to serve precompressed files to the browser without having to compress on demand, this is achieved by compressing/encoding your content at build time.

## Getting Started

### Precompress content
Static nonimage files have to be precompressed using [Zopfli](https://en.wikipedia.org/wiki/Zopfli) and/or [Brotli](https://en.wikipedia.org/wiki/Brotli), see the example for how to do it with gulp.
The files must have the exact same filename as the source + `.br` or `.gzip` (`index.html` would be `index.html.br` for the Brotli version).

### Encode images
Modern browsers support new image formats like webp and avif they can store more pixels per byte.
You can convert your images using the following tools [webp](https://developers.google.com/speed/webp/download) and [libavif](https://github.com/AOMediaCodec/libavif).
The files must have the same filename as the source but with a new file extension (`image.jpg` would be `image.webp` for the webp version).

### ASP.NET
Add `AddCompressedStaticFiles()` in your `Startup.ConfigureServices()` method.
Replace `UseStaticFiles();` with `UseCompressedStaticFiles();` in `Startup.Configure()`.
By default CompressedStaticFiles is configured to allow slightly larger files for some image formats as they can store more pixels per byte, this can be disabled by calling `CompressedStaticFileOptions.RemoveImageSubstitutionCostRatio()`.

## Example
An example can be found in the [Example](https://github.com/material-blazor/CompressedStaticFiles.AspNetCore/tree/main/CompressedStaticFiles.Example) directory.
By using this package the Lighthouse mobile performance went from `76` to `98` and the transferred size went from `526 kb` to `141 kb`.

## Acknowledgements
    
This solution is based upon a clone of https://github.com/AnderssonPeter/CompressedStaticFiles.
    
Which was based upon @neyromant from the following issue https://github.com/aspnet/Home/issues/1584#issuecomment-227455026.
