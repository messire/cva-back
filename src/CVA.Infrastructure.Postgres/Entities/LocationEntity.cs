namespace CVA.Infrastructure.Postgres;

/// <summary>
/// EF Core owned type for representing a location.
/// </summary>
internal sealed class LocationEntity
{
    /// <summary>
    /// City name.
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// Country name.
    /// </summary>
    public string? Country { get; set; }
}