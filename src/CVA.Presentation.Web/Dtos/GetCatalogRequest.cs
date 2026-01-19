namespace CVA.Presentation.Web.Dtos;

/// <summary>
/// Catalog query parameters.
/// </summary>
public class GetCatalogRequest
{
    /// <summary>
    /// Free-text search query.
    /// </summary>
    [FromQuery]
    public string? Search { get; set; }

    /// <summary>
    /// Required skill tags.
    /// </summary>
    [FromQuery]
    public string[]? Skills { get; set; }

    /// <summary>
    /// Filter by OpenToWork flag.
    /// </summary>
    [FromQuery]
    public bool? OpenToWork { get; set; }

    /// <summary>
    /// Filter by verification status.
    /// </summary>
    [FromQuery]
    public string? VerificationStatus { get; set; }

    /// <summary>
    /// Pagination configuration.
    /// </summary>
    [FromQuery]
    public int? Page { get; set; }

    /// <summary>
    /// Page size.
    /// </summary>
    [FromQuery]
    public int? PageSize { get; set; }

    /// <summary>
    /// Sorting field.
    /// </summary>
    [FromQuery]
    public string? SortField { get; set; }

    /// <summary>
    /// Sorting order.
    /// </summary>
    [FromQuery]
    public string? SortOrder { get; set; }
}