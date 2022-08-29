# CompressedStaticFiles.AspNet


---



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
Static nonimage files have to be precompressed using [Gzip](https://en.wikipedia.org/wiki/Gzip) and/or [Brotli](https://en.wikipedia.org/wiki/Brotli), see the example CompressedStaticFiles.csproj for an automated methodology for producing compressed css & js files.
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
    
This solution is developed from a clone of [AnderssonPeter/CompressedStaticFiles](https://github.com/AnderssonPeter/CompressedStaticFiles),
which was based upon work by [@neyromant](https://github.com/neyromant) from the following issue [ASP.NET Issue #1584](https://github.com/aspnet/Home/issues/1584#issuecomment-227455026).

We built this cloned project because:

- We wanted to update to the currently supported version of .NET;
- To make some refinements that were to our taste; and
- To add MSBuild code to the [example CSPROJ file](https://github.com/Material-Blazor/CompressedStaticFiles.AspNetCore/blob/main/CompressedStaticFiles.Example/CompressedStaticFiles.Example.csproj#L37) to build Brotli and Gzip compressed CSS and JS files, using a method contributed by [SQL-MisterMagoo](https://github.com/SQL-MisterMagoo).