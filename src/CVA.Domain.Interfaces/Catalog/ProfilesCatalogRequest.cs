namespace CVA.Domain.Interfaces;


/// <summary>
/// Catalog query request for profiles.
/// </summary>
public sealed class ProfilesCatalogRequest
{
    /// <summary>
    /// Free-text search query.
    /// </summary>
    public string? Search { get; init; }

    /// <summary>
    /// Required skill tags.
    /// </summary>
    public string[]? Skills { get; init; }

    /// <summary>
    /// Filter by OpenToWork flag.
    /// </summary>
    public bool? OpenToWork { get; init; }

    /// <summary>
    /// Filter by verification status.
    /// </summary>
    public VerificationStatus? VerificationStatus { get; init; }

    /// <summary>
    /// Sorting configuration.
    /// </summary>
    public required ProfilesCatalogSort Sort { get; init; }

    /// <summary>
    /// Pagination configuration.
    /// </summary>
    public required PageRequest Page { get; init; }
}