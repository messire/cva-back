namespace CVA.Application.ProfileService;

/// <summary>
/// Handles the query to get a developer profile by its identifier.
/// </summary>
/// <param name="repository">The developer profile repository.</param>
public sealed class GetProfileByIdHandler(IDeveloperProfileRepository repository)
    : IQueryHandler<GetProfileByIdQuery, ProfileDto>
{
    /// <inheritdoc />
    public async Task<Result<ProfileDto>> HandleAsync(GetProfileByIdQuery query, CancellationToken ct)
    {
        var profile = await repository.GetByIdAsync(query.Id, ct);
        return profile is not null
            ? profile.ToDto()
            : AppError.NotFound($"Profile with id {query.Id} was not found.");
    }
}