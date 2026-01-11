using CVA.Infrastructure.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CVA.Presentation.Auth;

/// <summary>
/// Dependency injection registration for Auth.
/// </summary>
public static class DiConfig
{
    /// <summary>
    /// Registers backend-driven OAuth redirect authentication services.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="configuration">Application configuration.</param>
    public static void AddPresentationAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        
        services.AddSingleton(provider => provider.GetRequiredService<IOptions<GoogleAuthOptions>>().Value);

        services.AddMemoryCache();
        services.AddSingleton<IOAuthStateStore, OAuthStateStore>();
        services.AddSingleton<IOneTimeCodeStore, OneTimeCodeStore>();

        services.AddSingleton(provider =>
        {
            var options = provider.GetRequiredService<GoogleAuthOptions>();
            return new GoogleAuthorizeUrlBuilder(options.ClientId);
        });

        services.AddHttpClient<IGoogleOAuthClient, GoogleOAuthClient>();
        services.AddScoped<IGoogleOAuthFlow, GoogleOAuthFlow>();
    }
}