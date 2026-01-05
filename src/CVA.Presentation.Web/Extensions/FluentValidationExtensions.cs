using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CVA.Presentation.Web;

/// <summary>
/// Provides extension methods to integrate FluentValidation into an ASP.NET Core application.
/// </summary>
public static class FluentValidationExtensions
{
    /// <param name="services">The <see cref="IServiceCollection"/> to which the validators should be added.</param>
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Configures services to register FluentValidation validators from the assembly containing the specified type.
        /// </summary>
        /// <typeparam name="T">A type whose assembly will be searched for validators.</typeparam>
        public void AddValidatorsFromAssemblyContaining<T>()
        {
            var assembly = typeof(T).Assembly;
            var validatorType = typeof(IValidator<>);

            var validators = assembly.GetTypes()
                .Where(type => type is { IsAbstract: false, IsGenericTypeDefinition: false })
                .SelectMany(type => type.GetInterfaces(), (type, i) => new { Type = type, Interface = i })
                .Where(arg => arg.Interface.IsGenericType && arg.Interface.GetGenericTypeDefinition() == validatorType);

            foreach (var validator in validators)
            {
                services.AddScoped(validator.Interface, validator.Type);
            }
        }

        /// <summary>
        /// Configures services to enable FluentValidation automatic validation for models in the application.
        /// </summary>
        /// <returns>The same <see cref="IServiceCollection"/> instance, enabling method chaining.</returns>
        public void AddFluentValidationAutoValidation()
        {
            services.Configure<ApiBehaviorOptions>(_ => { });
            services.AddControllers(options => options.Filters.Add<FluentValidationFilter>());
        }
    }
}

/// <summary>
/// Represents an ASP.NET Core action filter that performs FluentValidation-based validation on action method arguments.
/// </summary>
public class FluentValidationFilter : IAsyncActionFilter
{
    /// <inheritdoc />
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument == null) continue;

            var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
            if (context.HttpContext.RequestServices.GetService(validatorType) is not IValidator validator) continue;

            var validationContext = new ValidationContext<object>(argument);
            var result = await validator.ValidateAsync(validationContext, context.HttpContext.RequestAborted);
            if (result.IsValid) continue;

            foreach (var error in result.Errors)
            {
                context.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }

        if (!context.ModelState.IsValid)
        {
            context.Result = new BadRequestObjectResult(context.ModelState);
            return;
        }

        await next();
    }
}
