namespace CVA.Application.ProfileService;

/// <summary>
/// Handles the retrieval of the current authenticated developer's profile.
/// </summary>
public sealed class GetProfileHandler(IDeveloperProfileRepository repository, ICurrentUserAccessor currentUserAccessor)
    : IQueryHandler<GetProfileQuery, ProfileDto>
{
    /// <inheritdoc />
    public async Task<Result<ProfileDto>> HandleAsync(GetProfileQuery query, CancellationToken ct)
    {
        if (!currentUserAccessor.IsAuthenticated)
        {
            return AppError.Failure("User is not authenticated.");
        }

        var profile = await repository.GetByIdAsync(currentUserAccessor.UserId, ct);
        return profile is not null
            ? profile.ToDto()
            : AppError.NotFound($"Profile for user {currentUserAccessor.UserId} was not found.");
    }
}