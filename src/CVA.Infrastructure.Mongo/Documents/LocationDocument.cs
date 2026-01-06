namespace CVA.Infrastructure.Mongo;

/// <summary>
/// Represents a MongoDB embedded document for a location.
/// </summary>
internal sealed class LocationDocument
{
    /// <summary>
    /// The city of the location.
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// The country of the location.
    /// </summary>
    public string? Country { get; set; }
}