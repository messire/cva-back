using System.Reflection;

namespace CVA.Tests.Common;

/// <summary>
/// A custom specimen builder for creating <see cref="YearsOfExperience"/> instances.
/// Ensures that generated values are within the valid range (0-80).
/// </summary>
public sealed class YearsOfExperienceBuilder : ISpecimenBuilder
{
    /// <summary>
    /// Gets the singleton instance of the <see cref="YearsOfExperienceBuilder"/> class.
    /// </summary>
    public static YearsOfExperienceBuilder Instance => new();

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

        return type == typeof(YearsOfExperience)
            ? YearsOfExperience.From(Random.Shared.Next(0, 81))
            : new NoSpecimen();
    }
}