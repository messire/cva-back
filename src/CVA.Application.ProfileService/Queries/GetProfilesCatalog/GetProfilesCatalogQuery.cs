namespace CVA.Application.ProfileService;

/// <summary>
/// Query to get the catalog of developer profiles with optional filtering.
/// </summary>
/// <param name="Search">The search string to filter profiles by name or title.</param>
/// <param name="Skills">The set of skills to filter profiles.</param>
/// <param name="OpenToWork">Filter profiles by their "open to work" status.</param>
/// <param name="VerificationStatus">Filter profiles by their verification status.</param>
public sealed record GetProfilesCatalogQuery(string? Search, string[] Skills, bool? OpenToWork, string? VerificationStatus)
    : IQuery<ProfileCardDto[]>;