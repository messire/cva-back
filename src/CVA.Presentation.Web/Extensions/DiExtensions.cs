using CVA.Application.Services;
using CVA.Application.Validators;
using CVA.Infrastructure.Common;
using CVA.Infrastructure.Mongo;
using CVA.Infrastructure.Postgres;
using CVA.Tools.Common;

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
        /// Registers the necessary services for the API layer, including controllers and OpenAPI support.
        /// </summary>
        public void RegisterApiServices()
        {
            builder.Services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            });
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
}