using System.Collections.Generic;

namespace CompressedStaticFiles.AspNet;


/// <summary>
/// Options for compressed static files.
/// </summary>
public class CompressedStaticFileOptions
{
    /// <summary>
    /// Pre-compressed static files are delivered in responses when set to true.
    /// </summary>
    public bool EnablePrecompressedFiles { get; set; } = true;


    /// <summary>
    /// Images are substituted for more efficient responses when set to true.
    /// </summary>
    public bool EnableImageSubstitution { get; set; } = true;


    /// <summary>
    /// Used to prioritize image formats that contain higher quality per byte, if only size should be considered remove all entries.
    /// </summary>
    public readonly Dictionary<string, float> ImageSubstitutionCostRatio = new()
    {
        { "image/bmp", 2 },
        { "image/tiff", 1 },
        { "image/gif", 1 },
        { "image/apng", 0.9f },
        { "image/png", 0.9f },
        { "image/webp", 0.9f },
        { "image/avif", 0.8f }
    };
}
