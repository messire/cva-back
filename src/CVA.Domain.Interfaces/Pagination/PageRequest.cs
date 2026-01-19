namespace CVA.Domain.Interfaces;

/// <summary>
/// Represents pagination parameters for a query.
/// </summary>
public sealed class PageRequest
{
    /// <summary>
    /// Page number (1-based).
    /// </summary>
    public required int Number { get; init; }

    /// <summary>
    /// Page size.
    /// </summary>
    public required int Size { get; init; }
}