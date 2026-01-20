namespace CVA.Application.Contracts;

/// <summary>
/// Standard catalog response wrapper with items, pagination metadata,
/// and applied sorting information.
/// </summary>
/// <typeparam name="T">Catalog item DTO type.</typeparam>
public sealed class CatalogResponseDto<T>
{
    /// <summary>
    /// Catalog items for the requested page.
    /// </summary>
    public required T[] Items { get; init; }

    /// <summary>
    /// Pagination metadata.
    /// </summary>
    public required PaginationDto Pagination { get; init; }

    /// <summary>
    /// Applied sorting information.
    /// </summary>
    public required SortingDto Sorting { get; init; }
}