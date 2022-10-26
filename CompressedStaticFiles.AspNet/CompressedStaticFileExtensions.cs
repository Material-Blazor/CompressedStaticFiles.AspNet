using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace CompressedStaticFiles.AspNet;


/// <summary>
/// Static extensions.
/// </summary>
public static class CompressedStaticFileExtensions
{
    /// <summary>
    /// Removes substitution cost ratios.
    /// </summary>
    /// <param name="compressedStaticFileOptions"></param>
    /// <returns></returns>
    public static CompressedStaticFileOptions RemoveImageSubstitutionCostRatio(this CompressedStaticFileOptions compressedStaticFileOptions)
    {
        compressedStaticFileOptions.ImageSubstitutionCostRatio.Clear();
        return compressedStaticFileOptions;
    }

    
    /// <summary>
    /// Adds the compressed and image alternative file provider services as singletons.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddCompressedStaticFiles(this IServiceCollection services)
    {
        services.AddSingleton<IAlternativeFileProvider, CompressedAlternativeFileProvider>();
        services.AddSingleton<IAlternativeFileProvider, AlternativeImageFileProvider>();
        return services;
    }


    /// <summary>
    /// Adds the compressed and image alternative file provider services as singletons.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configureOptions"></param>
    /// <returns></returns>
    public static IServiceCollection AddCompressedStaticFiles(this IServiceCollection services, Action<CompressedStaticFileOptions> configureOptions)
    {
        services.Configure(configureOptions);
        services.AddSingleton<IAlternativeFileProvider, CompressedAlternativeFileProvider>();
        services.AddSingleton<IAlternativeFileProvider, AlternativeImageFileProvider>();
        return services;
    }


    /// <summary>
    /// Middleware to use compressed static assets. Substitute this for <see cref="IApplicationBuilder.UseStaticAssets"/>.
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IApplicationBuilder UseCompressedStaticFiles(this IApplicationBuilder app)
    {
        if (app == null)
        {
            throw new ArgumentNullException(nameof(app));
        }

        return app.UseMiddleware<CompressedStaticFileMiddleware>();
    }


    /// <summary>
    /// Middleware to use compressed static assets. Substitute this for <see cref="IApplicationBuilder.UseStaticAssets"/>.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="staticFileOptions"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IApplicationBuilder UseCompressedStaticFiles(this IApplicationBuilder app, StaticFileOptions staticFileOptions)
    {
        if (app == null)
        {
            throw new ArgumentNullException(nameof(app));
        }

        return app.UseMiddleware<CompressedStaticFileMiddleware>(Options.Create(staticFileOptions));
    }
}
