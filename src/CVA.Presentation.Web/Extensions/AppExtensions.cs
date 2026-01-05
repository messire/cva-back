using Scalar.AspNetCore;

namespace CVA.Presentation.Web;

/// <summary>
/// Provides extension methods for application-level operations in the CVA.Presentation.Web namespace.
/// </summary>
internal static class AppExtensions
{
    extension(WebApplication app)
    {
        /// <summary>
        /// Configures the development environment for the application.
        /// </summary>
        public void ConfigureDevEnv()
        {
            if (!app.Environment.IsDevelopment() && !app.Configuration.GetValue<bool>("EnableOpenApi")) return;
            app.MapOpenApi();
            app.MapScalarApiReference("/docs");
        }

        /// <summary>
        /// Configures the API for the application by enabling HTTPS redirection
        /// and mapping the controllers to their respective endpoints.
        /// </summary>
        public void ConfigureApi()
        {
            app.UseHttpsRedirection();
            app.MapControllers();
        }
    }
}