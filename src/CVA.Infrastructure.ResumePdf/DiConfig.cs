using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CVA.Infrastructure.ResumePdf;

/// <summary>
/// Dependency injection extensions for resume PDF generation.
/// </summary>
public static class DiConfig
{
    /// <summary>
    /// Adds resume PDF generation and caching in S3 storage.
    /// Requires S3 storage to be registered (AddCvaS3Storage).
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="configuration">Application configuration.</param>
    public static IServiceCollection AddCvaResumePdf(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<ResumePdfOptions>()
            .Bind(configuration.GetSection(ResumePdfOptions.Path))
            .Validate(options => !string.IsNullOrWhiteSpace(options.FrontendBaseUrl), "ResumePdf:FrontendBaseUrl is required.")
            .ValidateOnStart();

        services.AddSingleton<PlaywrightResumePdfRenderer>();
        services.AddSingleton<ResumePdfService>();
        services.AddHostedService<ResumePdfStartupCheck>();

        return services;
    }
}