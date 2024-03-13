using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IHost = Microsoft.AspNetCore.Hosting.IWebHostEnvironment;


namespace CompressedStaticFiles.AspNet;


/// <summary>
/// Compressed static files middleware.
/// </summary>
public class CompressedStaticFileMiddleware
{
    private readonly AsyncLocal<IFileAlternative> _alternativeFile = new();
    private readonly IOptions<StaticFileOptions> _staticFileOptions;
    private readonly IEnumerable<IAlternativeFileProvider> _alternativeFileProviders;
    private readonly StaticFileMiddleware _staticFileMiddleware;
    private readonly ILogger _logger;


    public CompressedStaticFileMiddleware(
        RequestDelegate next,
        IHost hostingEnv,
        IOptions<StaticFileOptions> staticFileOptions, 
        ILoggerFactory loggerFactory, 
        IEnumerable<IAlternativeFileProvider> alternativeFileProviders)
    {
        if (next == null)
        {
            throw new ArgumentNullException(nameof(next));
        }

        if (loggerFactory == null)
        {
            throw new ArgumentNullException(nameof(loggerFactory));
        }

        if (hostingEnv == null)
        {
            throw new ArgumentNullException(nameof(hostingEnv));
        }

        if (!alternativeFileProviders.Any())
        {
            throw new Exception("No IAlternativeFileProviders where found, did you forget to add AddCompressedStaticFiles() in ConfigureServices?");
        }
        
        _logger = loggerFactory.CreateLogger<CompressedStaticFileMiddleware>();

        _staticFileOptions = staticFileOptions ?? throw new ArgumentNullException(nameof(staticFileOptions));
        _alternativeFileProviders = alternativeFileProviders;
        InitializeStaticFileOptions(hostingEnv, staticFileOptions);

        _staticFileMiddleware = new StaticFileMiddleware(next, hostingEnv, staticFileOptions, loggerFactory);
    }


    private void InitializeStaticFileOptions(IHost hostingEnv, IOptions<StaticFileOptions> staticFileOptions)
    {
        staticFileOptions.Value.FileProvider = staticFileOptions.Value.FileProvider ?? hostingEnv.WebRootFileProvider;
        var contentTypeProvider = staticFileOptions.Value.ContentTypeProvider ?? new FileExtensionContentTypeProvider();
        
        if (contentTypeProvider is FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            foreach(var alternativeFileProvider in _alternativeFileProviders)
            {
                alternativeFileProvider.Initialize(fileExtensionContentTypeProvider);
            }
            
        }

        staticFileOptions.Value.ContentTypeProvider = contentTypeProvider;

        var originalPrepareResponse = staticFileOptions.Value.OnPrepareResponse;
        
        staticFileOptions.Value.OnPrepareResponse = context =>
        {
            originalPrepareResponse(context);
            var alternativeFile = this._alternativeFile.Value;
            
            if (alternativeFile != null)
            {
                alternativeFile.Prepare(contentTypeProvider, context);
            }
            
        };
    }


    /// <summary>
    /// Processes a request to determine if it matches a known file, and if so, serves it. If there is an appropriate
    /// compressed alternative file, it is served instead.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public Task Invoke(HttpContext context)
    {
        if (context.Request.Path.HasValue)
        {
            ProcessRequest(context);
        }

        return _staticFileMiddleware.Invoke(context);
    }


    private void ProcessRequest(HttpContext context)
    {
        var fileSystem = _staticFileOptions.Value.FileProvider;

        var originalFile = fileSystem.GetFileInfo(context.Request.Path);

        if (!originalFile.Exists || originalFile.IsDirectory)
        {
            return;
        }

        //Find the smallest file from all our alternative file providers
        var smallestAlternativeFile = _alternativeFileProviders.Select(alternativeFileProvider => alternativeFileProvider.GetAlternative(context, fileSystem, originalFile))
                                                              .Where(af => af != null)
                                                              .MinBy(alternativeFile => alternativeFile?.Cost);

        if (smallestAlternativeFile != null)
        {
            smallestAlternativeFile.Apply(context);
            _alternativeFile.Value = smallestAlternativeFile;
        }
    }
}
