using CVA.Application.Services;
using CVA.Application.Validators;
using CVA.Infrastructure.Common;
using CVA.Infrastructure.Mongo;
using CVA.Infrastructure.Postgres;
using CVA.Tools.Common;
using Microsoft.AspNetCore.Cors.Infrastructure;
using static System.StringSplitOptions;

namespace CVA.Presentation.Web;

/// <summary>
/// Provides a collection of extension methods to support dependency injection in the application.
/// </summary>
internal static class DiExtensions
{
    extension(WebApplicationBuilder builder)
    {
        /// <summary>
        /// Registers a specific configuration file to the application's configuration builder.
        /// </summary>
        /// <param name="configName">The name of the configuration file to be added.</param>
        public void RegisterConfig(string configName)
        {
            builder.Configuration.AddConfigFiles(configName, builder.Environment);
        }

        /// <summary>
        /// Configures and registers Cross-Origin Resource Sharing (CORS) policies for the application.
        /// </summary>
        public void RegisterCors()
        {
            var origins = builder.Configuration["CORS_ORIGINS"]?.Split(',', RemoveEmptyEntries | TrimEntries);
            if (!builder.Environment.IsDevelopment() && (origins == null || origins.Length == 0))
            {
                throw new InvalidOperationException("CORS_ORIGINS configuration is required for non-development environments.");
            }

            builder.Services.AddCors(options =>
                ConfigureCors(options, builder.Environment, origins));
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
            builder.Services.RegisterUserService();
        }

        /// <summary>
        /// Registers the necessary services for the application's database layer, including connection setup and registration.
        /// </summary>
        public void RegisterDatabase()
        {
            var dbType = builder.Configuration.GetRequiredSection(DatabaseOptions.Path).Get<DatabaseOptions>();
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
    }

    private static void ConfigureCors(CorsOptions options, IWebHostEnvironment env, string[]? origins)
    {
        options.AddPolicy("Frontend", policy =>
        {
            policy.ApplyEnvironmentOrigins(env.IsDevelopment(), origins)
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    }


    private static CorsPolicyBuilder ApplyEnvironmentOrigins(this CorsPolicyBuilder policy, bool isDevelopment, string[]? origins)
    {
        if (isDevelopment) return policy.AllowAnyOrigin();
            
        return origins is { Length: > 0 } 
            ? policy.WithOrigins(origins).AllowCredentials() 
            : throw new InvalidOperationException("Production origins missing");
    }
}