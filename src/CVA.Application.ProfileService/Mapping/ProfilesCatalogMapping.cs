namespace CVA.Application.ProfileService;

/// <summary>
/// Provides mapping helpers for profiles catalog query.
/// </summary>
public static class ProfilesCatalogMapping
{
    /// <summary>
    /// Maps current application query to repository catalog request.
    /// </summary>
    /// <param name="query">Catalog query.</param>
    /// <param name="page">Page number (1-based).</param>
    /// <param name="pageSize">Page size.</param>
    /// <param name="sortField">Sort field key.</param>
    /// <param name="sortOrder">Sort order key.</param>
    /// <returns>Repository catalog request.</returns>
    public static ProfilesCatalogRequest ToProfilesCatalogRequest(this GetProfilesCatalogQuery query, int page, int pageSize, string? sortField, string? sortOrder)
        => new()
        {
            Search = query.Search,
            Skills = query.Skills,
            OpenToWork = query.OpenToWork,
            VerificationStatus = MapVerificationStatus(query.VerificationStatus),
            Sort = MapSort(sortField, sortOrder),
            Page = ToPageRequest(page, pageSize),
        };

    /// <summary>
    /// Maps external verification status key to domain <see cref="VerificationStatus"/>.
    /// Null/empty means no filtering.
    /// </summary>
    /// <param name="value">External verification status key.</param>
    /// <returns>Verification status filter value or null.</returns>
    private static VerificationStatus? MapVerificationStatus(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        return Enum.TryParse<VerificationLevel>(value.Trim(), true, out var level)
            ? new VerificationStatus(level)
            : throw new ArgumentOutOfRangeException(nameof(value), value, "Unsupported verification status.");
    }

    /// <summary>
    /// Maps external sort keys to strongly typed sorting configuration.
    /// Applies defaults when values are null or whitespace.
    /// </summary>
    /// <param name="sortField">External sorting field key.</param>
    /// <param name="sortOrder">External sorting order key.</param>
    /// <returns>Strongly typed sorting configuration.</returns>
    private static ProfilesCatalogSort MapSort(string? sortField, string? sortOrder)
    {
        var fieldKey = string.IsNullOrWhiteSpace(sortField)
            ? ProfilesSortFields.UpdatedAt
            : sortField.Trim();

        var orderKey = string.IsNullOrWhiteSpace(sortOrder)
            ? SortOrders.Desc
            : sortOrder.Trim();

        var field = fieldKey switch
        {
            ProfilesSortFields.Name => ProfilesSortField.Name,
            ProfilesSortFields.Id => ProfilesSortField.Id,
            _ => ProfilesSortField.UpdatedAt
        };

        var order = orderKey switch
        {
            SortOrders.Asc => SortOrder.Asc,
            _ => SortOrder.Desc
        };

        return new ProfilesCatalogSort
        {
            Field = field,
            Order = order
        };
    }

    private static PageRequest ToPageRequest(int number, int size)
        => new() { Number = number, Size = size };
}