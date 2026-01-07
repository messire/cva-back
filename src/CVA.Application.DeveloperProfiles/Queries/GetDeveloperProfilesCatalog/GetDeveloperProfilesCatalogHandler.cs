namespace CVA.Application.DeveloperProfiles;

/// <summary>
/// Handles the query to get the catalog of developer profiles.
/// </summary>
/// <param name="repository">The developer profile repository.</param>
public sealed class GetDeveloperProfilesCatalogHandler(IDeveloperProfileRepository repository) 
    : IQueryHandler<GetDeveloperProfilesCatalogQuery, DeveloperProfileCardDto[]>
{
    /// <inheritdoc />
    public async Task<Result<DeveloperProfileCardDto[]>> HandleAsync(GetDeveloperProfilesCatalogQuery query, CancellationToken ct)
    {
        var profiles = await repository.GetAllAsync(ct);
        var filtered = profiles.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();
            filtered = filtered.Where(profile => 
                profile.Name.FirstName.Contains(search, StringComparison.OrdinalIgnoreCase) || 
                profile.Name.LastName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                (profile.Role != null && profile.Role.Value.Contains(search, StringComparison.OrdinalIgnoreCase)));
        }

        if (query.Skills is { Length: > 0 })
        {
            filtered = filtered
                .Where(profile => query.Skills
                    .All(value => profile.Skills.Any(tag => tag.Value.Equals(value, StringComparison.OrdinalIgnoreCase))));
        }

        if (query.OpenToWork.HasValue)
        {
            filtered = filtered.Where(profile => profile.OpenToWork.Value == query.OpenToWork.Value);
        }

        if (!string.IsNullOrWhiteSpace(query.VerificationStatus))
        {
            var status = VerificationStatus.TryFrom(query.VerificationStatus);
            filtered = filtered.Where(profile => profile.Verification.Value == status.Value);
        }

        return filtered.Select(profile => profile.ToCardDto()).ToArray();
    }
}