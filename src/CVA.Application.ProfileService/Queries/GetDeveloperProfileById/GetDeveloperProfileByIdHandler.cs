namespace CVA.Application.ProfileService;

/// <summary>
/// Handles the query to get a developer profile by its identifier.
/// </summary>
/// <param name="repository">The developer profile repository.</param>
public sealed class GetDeveloperProfileByIdHandler(IDeveloperProfileRepository repository)
    : IQueryHandler<GetDeveloperProfileByIdQuery, DeveloperProfileDto>
{
    /// <inheritdoc />
    public async Task<Result<DeveloperProfileDto>> HandleAsync(GetDeveloperProfileByIdQuery query, CancellationToken ct)
    {
        var profile = await repository.GetByIdAsync(query.Id, ct);
        return profile is not null
            ? profile.ToDto()
            : AppError.NotFound($"Profile with id {query.Id} was not found.");
    }
}