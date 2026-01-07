namespace CVA.Application.Contracts;

/// <summary>
/// Data transfer object that represents a geographic location.
/// </summary>
public sealed record LocationDto
{
    /// <summary>
    /// City name.
    /// </summary>
    public string? City { get; init; }

    /// <summary>
    /// Country name.
    /// </summary>
    public string? Country { get; init; }
}