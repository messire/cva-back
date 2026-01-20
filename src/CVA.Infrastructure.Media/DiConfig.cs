using CVA.Application.Abstractions.Media;
using CVA.Infrastructure.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CVA.Infrastructure.Media;

/// <summary>
/// Dependency injection extensions for media storage.
/// </summary>
public static class DiConfig
{
    /// <summary>
    /// Adds local media storage (volume-based) services.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="configuration">Application configuration.</param>
    public static IServiceCollection AddCvaMediaStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<MediaOptions>()
            .Bind(configuration.GetSection(MediaOptions.Path))
            .ValidateOnStart();
        
        services.AddSingleton<IMediaStorage, LocalMediaStorage>();

        return services;
    }
}