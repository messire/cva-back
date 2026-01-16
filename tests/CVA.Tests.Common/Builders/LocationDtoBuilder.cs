using CVA.Application.Contracts;

namespace CVA.Tests.Common;

/// <summary>
/// A custom specimen builder for creating <see cref="LocationDto"/> instances.
/// Ensures that generated values are short and valid for validation rules.
/// </summary>
public sealed class LocationDtoBuilder : ISpecimenBuilder
{
    /// <summary>
    /// Gets the singleton instance of the <see cref="LocationDtoBuilder"/> class.
    /// </summary>
    public static LocationDtoBuilder Instance => new();

    /// <inheritdoc />
    public object Create(object request, ISpecimenContext context)
    {
        if (request is not Type type || type != typeof(LocationDto)) return new NoSpecimen();

        return new LocationDto
        {
            City = CreateShortName(context, "City"),
            Country = CreateShortName(context, "Country")
        };
    }

    private static string CreateShortName(ISpecimenContext context, string prefix)
        => $"{prefix}-{context.Create<Guid>().ToString("N")[..8]}";
}
