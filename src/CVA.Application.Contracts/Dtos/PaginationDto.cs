namespace CVA.Application.Contracts;

/// <summary>
/// Pagination metadata for catalog responses.
/// </summary>
public sealed class PaginationDto
{
    /// <summary>
    /// Current page number (1-based).
    /// </summary>
    public required int Number { get; init; }

    /// <summary>
    /// Page size.
    /// </summary>
    public required int Size { get; init; }

    /// <summary>
    /// Total number of items matching the applied filters.
    /// </summary>
    public required long TotalCount { get; init; }

    /// <summary>
    /// Total number of pages based on TotalCount and Size.
    /// </summary>
    public required int TotalPages { get; init; }
}