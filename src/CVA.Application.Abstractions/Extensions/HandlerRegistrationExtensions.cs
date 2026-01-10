using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace CVA.Application.Abstractions.Extensions;

/// <summary>
/// DI helpers for registering command/query handlers via reflection.
/// </summary>
public static class HandlerRegistrationExtensions
{
    /// <summary>
    /// Registers all implementations of <see cref="ICommandHandler{TCommand,TResponse}"/> and
    /// <see cref="IQueryHandler{TQuery,TResponse}"/> found in the provided assembly.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="assembly">Assembly to scan.</param>
    /// <returns>Same service collection.</returns>
    public static void RegisterHandlersFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var types = assembly.GetTypes();

        foreach (var implementationType in types.Where(type => type is { IsClass: true, IsAbstract: false }))
        {
            foreach (var serviceType in implementationType.GetInterfaces())
            {
                if (!serviceType.IsGenericType) continue;

                var typeDefinition = serviceType.GetGenericTypeDefinition();
                if (typeDefinition != typeof(ICommandHandler<,>) && typeDefinition != typeof(IQueryHandler<,>)) continue;
                
                services.AddScoped(serviceType, implementationType);
            }
        }
    }
}