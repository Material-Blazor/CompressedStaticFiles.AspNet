using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;

namespace CompressedStaticFiles.AspNet;

/// <summary>
/// Information detailing alternative compressed versions of the requested file.
/// </summary>
public interface IFileAlternative
{
    /// <summary>
    /// The alternative file's size.
    /// </summary>
    long Size { get; }
    
    
    /// <summary>
    /// Used to give some files a higher priority.
    /// </summary>
    float Cost { get; }


    /// <summary>
    /// Applies this alternative file to the <see cref="HttpContext.Request"/>.
    /// </summary>
    /// <param name="context"></param>
    void Apply(HttpContext context);


    /// <summary>
    /// Prepares the <see cref="HttpContext.Response"/> content encoding for this alternative file.
    /// </summary>
    /// <param name="contentTypeProvider"></param>
    /// <param name="staticFileResponseContext"></param>
    void Prepare(IContentTypeProvider contentTypeProvider, StaticFileResponseContext staticFileResponseContext);
}
