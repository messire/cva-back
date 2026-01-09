using System.Text;
using CVA.Presentation.Web.Auth.Claims;
using CVA.Presentation.Web.Auth.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace CVA.Presentation.Web;

/// <summary>
/// Provides extension methods to configure authentication and authorization.
/// </summary>
internal static class AuthExtensions
{
    extension(WebApplicationBuilder builder)
    {
        /// <summary>
        /// Registers JWT authentication/authorization infrastructure.
        /// </summary>
        public void RegisterAuth()
        {
            builder.Services
                .AddOptions<JwtOptions>()
                .Bind(builder.Configuration.GetRequiredSection(JwtOptions.Path))
                .Validate(options =>
                    !string.IsNullOrWhiteSpace(options.Issuer) &&
                    !string.IsNullOrWhiteSpace(options.Audience) &&
                    !string.IsNullOrWhiteSpace(options.SigningKey) &&
                    options.LifetimeMinutes > 0, $"Invalid '{JwtOptions.Path}' configuration.")
                .ValidateOnStart();

            builder.Services.AddSingleton<JwtTokenGenerator>();

            var jwt = builder.Configuration.GetRequiredSection(JwtOptions.Path).Get<JwtOptions>();
            ArgumentNullException.ThrowIfNull(jwt);

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SigningKey));

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
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

                        NameClaimType = CustomClaimTypes.Subject,
                        RoleClaimType = CustomClaimTypes.Role
                    };
                });

            builder.Services.AddAuthorization();
        }
    }
}
