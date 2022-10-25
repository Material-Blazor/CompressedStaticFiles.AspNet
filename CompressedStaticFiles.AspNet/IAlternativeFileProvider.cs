using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;

namespace CompressedStaticFiles.AspNet;


/// <summary>
/// Provides and alternative file/
/// </summary>
public interface IAlternativeFileProvider
{
    /// <summary>
    /// Initializes the provider.
    /// </summary>
    /// <param name="fileExtensionContentTypeProvider"></param>
    void Initialize(FileExtensionContentTypeProvider fileExtensionContentTypeProvider);


    /// <summary>
    /// Returns the determined alternative file.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="fileSystem"></param>
    /// <param name="originalFile"></param>
    /// <returns></returns>
    IFileAlternative GetAlternative(HttpContext context, IFileProvider fileSystem, IFileInfo originalFile);
}
