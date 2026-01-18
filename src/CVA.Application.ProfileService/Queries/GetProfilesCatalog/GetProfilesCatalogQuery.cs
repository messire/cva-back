namespace CVA.Application.ProfileService;

/// <summary>
/// Query to get the catalog of profiles with paging, sorting and filters.
/// </summary>
/// <param name="Search">Free-text search query.</param>
/// <param name="Skills">Required skill tags (all must be present).</param>
/// <param name="OpenToWork">Filter profiles by OpenToWork flag.</param>
/// <param name="VerificationStatus">Filter profiles by verification status key.</param>
/// <param name="Page">Page number (1-based).</param>
/// <param name="PageSize">Page size.</param>
/// <param name="SortField">Sort field key (e.g. "updatedAt", "name", "id").</param>
/// <param name="SortOrder">Sort order key ("asc" or "desc").</param>
public sealed record GetProfilesCatalogQuery(
    string? Search,
    string[] Skills,
    bool? OpenToWork,
    string? VerificationStatus,
    int Page,
    int PageSize,
    string? SortField,
    string? SortOrder)
    : IQuery<CatalogResponseDto<ProfileCardDto>>;