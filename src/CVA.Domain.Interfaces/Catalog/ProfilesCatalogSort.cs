namespace CVA.Domain.Interfaces;

/// <summary>
/// Strongly typed sorting configuration for profiles catalog.
/// </summary>
public sealed class ProfilesCatalogSort
{
    /// <summary>
    /// Sorting field.
    /// </summary>
    public required ProfilesSortField Field { get; init; }

    /// <summary>
    /// Sorting order.
    /// </summary>
    public required SortOrder Order { get; init; }
}