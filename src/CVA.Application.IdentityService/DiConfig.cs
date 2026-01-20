using Microsoft.Extensions.DependencyInjection;

namespace CVA.Application.IdentityService;

/// <summary>
/// Provides dependency injection configuration for identity services.
/// </summary>
public static class DiConfig
{
    /// <summary>
    /// Registers identity application services.
    /// </summary>
    /// <param name="services">Service collection.</param>
    public static void RegisterIdentityService(this IServiceCollection services)
    {
        services.AddScoped<IIdentityService, IdentityService>();
    }
}