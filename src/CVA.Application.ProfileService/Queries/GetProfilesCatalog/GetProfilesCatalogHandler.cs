namespace CVA.Application.ProfileService;

/// <summary>
/// Handles the query to get the catalog of developer profiles.
/// </summary>
/// <param name="repository">The developer profile repository.</param>
public sealed class GetProfilesCatalogHandler(IDeveloperProfileRepository repository)
    : IQueryHandler<GetProfilesCatalogQuery, CatalogResponseDto<ProfileCardDto>>
{
    /// <inheritdoc />
    public async Task<Result<CatalogResponseDto<ProfileCardDto>>> HandleAsync(GetProfilesCatalogQuery query, CancellationToken ct)
    {
        var pageRequest = new PageRequest
        {
            Number = query.Page,
            Size = query.PageSize
        };

        var request = new ProfilesCatalogRequest
        {
            Search = query.Search,
            Skills = query.Skills is { Length: > 0 } ? query.Skills : null,
            OpenToWork = query.OpenToWork,
            VerificationStatus = MapVerificationStatus(query.VerificationStatus),
            Sort = MapSort(query.SortField, query.SortOrder),
            Page = pageRequest
        };

        var page = await repository.SearchCatalogAsync(request, ct);
        var items = page.Items
            .Select(profile => profile.ToCardDto())
            .ToArray();

        var totalPages = CalculateTotalPages(page.TotalCount, query.PageSize);
        var pagination = new PaginationDto
        {
            Number = query.Page,
            Size = query.PageSize,
            TotalCount = page.TotalCount,
            TotalPages = totalPages
        };

        var sorting = new SortingDto
        {
            Field = string.IsNullOrWhiteSpace(query.SortField)
                ? ProfilesSortFields.UpdatedAt
                : query.SortField.Trim(),
            Order = string.IsNullOrWhiteSpace(query.SortOrder)
                ? SortOrders.Desc
                : query.SortOrder.Trim()
        };

        var response = new CatalogResponseDto<ProfileCardDto>
        {
            Items = items,
            Pagination = pagination,
            Sorting = sorting
        };

        return response;
    }

    private static int CalculateTotalPages(long totalCount, int pageSize)
    {
        if (totalCount <= 0)
        {
            return 0;
        }

        var pages = (totalCount + pageSize - 1) / pageSize;
        return pages > int.MaxValue
            ? int.MaxValue
            : (int)pages;
    }

    private static VerificationStatus? MapVerificationStatus(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        Enum.TryParse<VerificationLevel>(value.Trim(), true, out var level);
        return new VerificationStatus(level);
    }

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
}