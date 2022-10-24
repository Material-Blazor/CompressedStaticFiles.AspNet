using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace CompressedStaticFiles;


/// <summary>
/// Implementation of <see cref="IAlternativeFileProvider"/> for compressed files such as CSS or JS.
/// </summary>
public class CompressedAlternativeFileProvider : IAlternativeFileProvider
{
    /// <summary>
    /// Dictionary of acceptable compression types.
    /// </summary>
    public static readonly ImmutableDictionary<string, string> CompressionTypes =
        new Dictionary<string, string>()
        {
            { "gzip", ".gz" },
            { "br", ".br" }
        }.ToImmutableDictionary();


    private readonly ILogger logger;
    private readonly IOptions<CompressedStaticFileOptions> options;


    /// <inheritdoc/>
    public CompressedAlternativeFileProvider(ILogger<CompressedAlternativeFileProvider> logger, IOptions<CompressedStaticFileOptions> options)
    {
        this.logger = logger;
        this.options = options;
    }


    /// <inheritdoc/>
    public void Initialize(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
    {
        // the StaticFileProvider would not serve the file if it does not know the content-type
        fileExtensionContentTypeProvider.Mappings[".br"] = "application/brotli";
    }


    /// <summary>
    /// Find the encodings that are supported by the browser and by this middleware
    /// </summary>
    private static IEnumerable<string> GetSupportedEncodings(HttpContext context)
    {
        var browserSupportedCompressionTypes = context.Request.Headers.GetCommaSeparatedValues("Accept-Encoding");
        var validCompressionTypes = CompressionTypes.Keys.Intersect(browserSupportedCompressionTypes, StringComparer.OrdinalIgnoreCase);
        return validCompressionTypes;
    }


    /// <inheritdoc/>
    public IFileAlternative GetAlternative(HttpContext context, IFileProvider fileSystem, IFileInfo originalFile)
    {
        if (!options.Value.EnablePrecompressedFiles)
        { 
            return null;
        }
        
        var supportedEncodings = GetSupportedEncodings(context);
        IFileInfo matchedFile = originalFile;
        
        foreach (var compressionType in supportedEncodings)
        {
            var fileExtension = CompressionTypes[compressionType];
            var file = fileSystem.GetFileInfo(context.Request.Path + fileExtension);
            
            if (file.Exists && file.Length < matchedFile.Length)
            {
                matchedFile = file;
            }
        }

        if (matchedFile != originalFile)
        {
            // a compressed version exists and is smaller, change the path to serve the compressed file
            var matchedPath = context.Request.Path.Value + Path.GetExtension(matchedFile.Name);
            
            return new CompressedAlternativeFile(logger, originalFile, matchedFile);                
        }

        return null;
    }
}
