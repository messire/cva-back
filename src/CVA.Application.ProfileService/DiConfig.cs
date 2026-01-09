using Microsoft.Extensions.DependencyInjection;

namespace CVA.Application.ProfileService;

/// <summary>
/// Dependency injection configuration for the developer profile application services.
/// </summary>
public static class DiConfig
{
    /// <summary>
    /// Registers developer profile application services into the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to register services into.</param>
    public static void RegisterDeveloperProfileService(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
    }
}