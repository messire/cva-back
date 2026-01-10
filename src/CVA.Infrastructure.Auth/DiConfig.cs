using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CVA.Infrastructure.Auth;

/// <summary>
/// Provides dependency injection configuration for the user service.
/// </summary>
public static class DiConfig
{
    /// <param name="services">The <see cref="IServiceCollection"/> to which the validators should be added.</param>
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Registers the user service implementation for dependency injection.
        /// </summary>
        /// <param name="configuration">The application configuration instance.</param>
        /// <param name="environment">Host environment.</param>
        public void RegisterAuthService(IConfiguration configuration, IHostEnvironment environment)
        {
            services.ConfigureJwtAuth(configuration, environment);
            services.ConfigureGoogleAuth(configuration, environment);
            services.ConfigureRefreshTokens(configuration, environment);
            services.AddAuthorization();
        }

        private void ConfigureJwtAuth(IConfiguration configuration, IHostEnvironment environment)
        {
            services.AddSingleton<IAppTokenIssuer, JwtTokenGenerator>();
            services.BindAndValidateJwtOptions(configuration);

            var jwt = configuration.GetRequiredSection(JwtOptions.Path).Get<JwtOptions>();
            ArgumentNullException.ThrowIfNull(jwt);

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => ConfigureJwtBearer(environment, options, jwt));
        }

        private void ConfigureGoogleAuth(IConfiguration configuration, IHostEnvironment environment)
        {
            services.AddSingleton<IGoogleTokenVerifier, GoogleIdTokenVerifier>();
            var section = configuration.GetSection(GoogleAuthOptions.Path);
            var clientId = section["ClientId"];
            if (string.IsNullOrWhiteSpace(clientId))
            {
                if (environment.IsDevelopment()) return;
                throw new InvalidOperationException("Auth:Google:ClientId is required in non-development environments.");
            }

            services.BindAndValidateGoogleAuth(configuration);
            services.AddSingleton(provider => provider.GetRequiredService<IOptions<GoogleAuthOptions>>().Value);
            services.AddSingleton<GoogleIdTokenVerifier>();
        }

        private void ConfigureRefreshTokens(IConfiguration configuration, IHostEnvironment environment)
        {
            services.BindAndValidateRefreshTokenOptions(configuration, environment);
            services.AddSingleton<IRefreshTokenProtector, RefreshTokenProtector>();
        }

        private void BindAndValidateGoogleAuth(IConfiguration configuration)
            => services
                .AddOptions<GoogleAuthOptions>()
                .Bind(configuration.GetRequiredSection(GoogleAuthOptions.Path))
                .Validate(options => !string.IsNullOrWhiteSpace(options.ClientId), "Auth:Google:ClientId is required.")
                .ValidateOnStart();

        private void BindAndValidateJwtOptions(IConfiguration configuration)
            => services
                .AddOptions<JwtOptions>()
                .Bind(configuration.GetRequiredSection(JwtOptions.Path))
                .Validate(options =>
                        !string.IsNullOrWhiteSpace(options.Issuer) &&
                        !string.IsNullOrWhiteSpace(options.Audience) &&
                        !string.IsNullOrWhiteSpace(options.SigningKey) &&
                        options.LifetimeMinutes > 0, $"Invalid '{JwtOptions.Path}' configuration.")
                .ValidateOnStart();

        private void BindAndValidateRefreshTokenOptions(IConfiguration configuration, IHostEnvironment environment)
            => services
                .AddOptions<RefreshTokenOptions>()
                .Bind(configuration.GetRequiredSection(RefreshTokenOptions.Path))
                .Validate(options
                        => options.LifetimeDays > 0, $"Invalid '{RefreshTokenOptions.Path}:LifetimeDays' configuration.")
                .Validate(options
                        => environment.IsDevelopment() || !string.IsNullOrWhiteSpace(options.Pepper), $"'{RefreshTokenOptions.Path}:Pepper' is required in non-development environments.")
                .ValidateOnStart();
    }

    private static void ConfigureJwtBearer(IHostEnvironment environment, JwtBearerOptions options, JwtOptions jwt)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SigningKey));

        options.RequireHttpsMetadata = !environment.IsDevelopment();
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt.Issuer,
            ValidateAudience = true,
            ValidAudience = jwt.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30),

            // custom claims
            NameClaimType = CustomClaimTypes.Subject,
            RoleClaimType = CustomClaimTypes.Role
        };
    }
}