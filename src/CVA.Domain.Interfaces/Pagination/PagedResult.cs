namespace CVA.Domain.Interfaces;

/// <summary>
/// Represents a paged query result with total item count.
/// Used for database-level pagination to ensure stable and deterministic results.
/// </summary>
/// <typeparam name="T">Type of the returned items.</typeparam>
public sealed class PagedResult<T>
{
    /// <summary>
    /// Items for the requested page.
    /// </summary>
    public required T[] Items { get; init; }

    /// <summary>
    /// Total number of items matching the applied filters before pagination is applied.
    /// </summary>
    public required long TotalCount { get; init; }
}