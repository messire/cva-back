using System.Reflection;

namespace CVA.Tests.Common;

/// <summary>
/// A custom specimen builder for creating <see cref="Url"/> instances.
/// Ensures that generated URLs are valid absolute URIs.
/// </summary>
public sealed class UrlBuilder : ISpecimenBuilder
{
    /// <summary>
    /// Gets the singleton instance of the <see cref="UrlBuilder"/> class.
    /// </summary>
    public static UrlBuilder Instance => new();

    /// <inheritdoc />
    public object Create(object request, ISpecimenContext context)
    {
        var type = request switch
        {
            PropertyInfo pi => pi.PropertyType,
            ParameterInfo pai => pai.ParameterType,
            Type t => t,
            _ => null
        };

        return type == typeof(Url)
            ? Url.From($"https://{context.Create<string>()}.test")
            : new NoSpecimen();
    }
}