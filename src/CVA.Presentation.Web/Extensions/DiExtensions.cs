using CVA.Application.IdentityService;
using CVA.Application.ProfileService;
using CVA.Application.Validators;
using CVA.Infrastructure.Auth;
using CVA.Infrastructure.Common;
using CVA.Infrastructure.Media;
using CVA.Infrastructure.Mongo;
using CVA.Infrastructure.Postgres;
using CVA.Infrastructure.ResumePdf;
using CVA.Infrastructure.Storage.S3;
using CVA.Presentation.Auth;
using CVA.Tools.Common;
using Microsoft.AspNetCore.Cors.Infrastructure;
using static System.StringSplitOptions;
using static Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders;

namespace CVA.Presentation.Web;

/// <summary>
/// Provides a collection of extension methods to support dependency injection in the application.
/// </summary>
internal static class DiExtensions
{
    extension(WebApplicationBuilder builder)
    {
        internal void RegisterConfig(string configName)
            => new Builder(builder).RegisterConfig(configName);

        internal void RegisterCors()
            => new Builder(builder).RegisterCors();

        internal void RegisterApiServices()
            => new Builder(builder).RegisterApiServices();

        internal void RegisterInnerServices()
            => new Builder(builder).RegisterInnerServices();

        internal void RegisterDatabase()
            => new Builder(builder).RegisterDatabase();

        internal void RegisterValidation()
            => new Builder(builder).RegisterValidation();

        internal void RegisterAuth()
            => new Builder(builder).RegisterAuth();
    }

    private sealed class Builder(WebApplicationBuilder builder)
    {
        /// <summary>
        /// Registers a specific configuration file to the application's configuration builder.
        /// </summary>
        /// <param name="configName">The name of the configuration file to be added.</param>
        public void RegisterConfig(string configName)
            => builder.Configuration.AddConfigFiles(configName, builder.Environment);

        /// <summary>
        /// Configures and registers Cross-Origin Resource Sharing (CORS) policies for the application.
        /// </summary>
        public void RegisterCors()
        {
            var origins = builder.Configuration["CORS_ORIGINS"];
            if (!builder.Environment.IsDevelopment() && string.IsNullOrEmpty(origins))
            {
                throw new InvalidOperationException("CORS_ORIGINS configuration is required for non-development environments.");
            }

            builder.Services.AddCors(options => ConfigureCors(options, builder.Environment, origins));
        }

        /// <summary>
        /// Registers the necessary services for the API layer, including controllers and OpenAPI support.
        /// </summary>
        public void RegisterApiServices()
        {
            builder.Services.AddControllers(options => { options.SuppressAsyncSuffixInActionNames = false; });
            builder.Services.AddOpenApi();
        }

        /// <summary>
        /// Registers the necessary services for the application's inner layers, such as business logic and data access.
        /// </summary>
        public void RegisterInnerServices()
        {
            builder.Services.RegisterDeveloperProfileService();
            builder.Services.RegisterIdentityService();

            builder.Services.AddScoped<CommandExecutor>();
            builder.Services.AddScoped<QueryExecutor>();

            builder.Services.Configure<MediaOptions>(builder.Configuration.GetSection(MediaOptions.Path));
            builder.Services.AddCvaS3Storage(builder.Configuration);
            builder.Services.AddCvaMediaStorage(builder.Configuration);
            builder.Services.AddCvaResumePdf(builder.Configuration);
        }

        /// <summary>
        /// Registers persistence services depending on the configured database type.
        /// </summary>
        public void RegisterDatabase()
        {
            var dbType = builder.Configuration.GetSection(DatabaseOptions.Path).Get<DatabaseOptions>();
            ArgumentNullException.ThrowIfNull(dbType);

            switch (dbType.Type)
            {
                case DatabaseType.Mongo:
                    builder.Services.RegisterMongo(builder.Configuration);
                    break;
                case DatabaseType.Postgres:
                    builder.Services.RegisterPostgres(builder.Configuration);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dbType.Type), dbType.Type, "Unsupported database type");
            }
        }

        /// <summary>
        /// Registers validation services for the application, including FluentValidation and validators from a specified assembly.
        /// </summary>
        public void RegisterValidation()
        {
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<IValidatorMarker>();
        }

        /// <summary>
        /// Registers authentication and authorization services.
        /// </summary>
        public void RegisterAuth()
        {
            builder.Services.RegisterAuthService(builder.Configuration, builder.Environment);
            builder.Services.AddPresentationAuth(builder.Configuration);
            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = XForwardedFor | XForwardedProto;
                options.KnownIPNetworks.Clear();
                options.KnownProxies.Clear();
            });
        }
    }

    private static void ConfigureCors(CorsOptions options, IWebHostEnvironment env, string? origins)
    {
        var allowedOrigins = origins?.Split(';', TrimEntries | RemoveEmptyEntries) ?? [];

        options.AddDefaultPolicy(policy =>
        {
            policy
                .ApplyEnvironmentOrigins(env.IsDevelopment(), allowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod();
        });

        options.AddPolicy("AuthExchange", policy =>
        {
            policy
                .ApplyEnvironmentOrigins(env.IsDevelopment(), allowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    }

    private static CorsPolicyBuilder ApplyEnvironmentOrigins(this CorsPolicyBuilder policy, bool isDevelopment, string[]? origins)
    {
        if (isDevelopment)
        {
            return origins?.Length > 0
                ? policy.WithOrigins(origins).AllowCredentials()
                : policy.AllowAnyOrigin();
        }

        return origins?.Length > 0
            ? policy.WithOrigins(origins).AllowCredentials()
            : throw new InvalidOperationException("Production origins missing");
    }
}