using Microsoft.Extensions.DependencyInjection;

namespace CVA.Application.Services;

/// <summary>
/// Provides dependency injection configuration for the application services.
/// </summary>
public static class DiConfig
{
    /// <summary>
    /// Registers the user service implementation for dependency injection.
    /// </summary>
    /// <param name="services">The service collection to which the user service will be added.</param>
    public static void RegisterUserService(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
    }
}