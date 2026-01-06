namespace CVA.Domain.Models;

/// <summary>
/// Represents the developer's location.
/// </summary>
/// <param name="City">The city where the developer resides.</param>
/// <param name="Country">The country where the developer resides.</param>
public sealed record Location(string City, string Country)
{
    /// <summary>
    /// Creates a new instance of <see cref="Location"/>.
    /// </summary>
    /// <param name="city">The city where the developer resides.</param>
    /// <param name="country">The country where the developer resides.</param>
    /// <returns>The created location.</returns>
    public static Location From(string city, string country)
        => new(Ensure.TrimToNull(city, nameof(city)), Ensure.TrimToNull(country, nameof(country)));

    /// <summary>
    /// Tries to create a new instance of <see cref="Location"/>.
    /// </summary>
    /// <param name="city">The city where the developer resides.</param>
    /// <param name="country">The country where the developer resides.</param>
    /// <returns>The created location or null if both city and country are null or empty.</returns>
    public static Location? TryFrom(string? city, string? country)
    {
        if (string.IsNullOrWhiteSpace(city) && string.IsNullOrWhiteSpace(country))
            return null;

        return new Location(city?.Trim() ?? string.Empty, country?.Trim() ?? string.Empty);
    }
}