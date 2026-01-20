namespace CVA.Application.ProfileService;

/// <summary>
/// Handles the update of header information in the developer profile.
/// </summary>
/// <param name="repository">The developer profile repository.</param>
/// <param name="userAccessor">The current user accessor.</param>
public sealed class UpdateProfileHeaderHandler(IDeveloperProfileRepository repository, ICurrentUserAccessor userAccessor) 
    : ICommandHandler<UpdateProfileHeaderCommand, ProfileDto>
{
    /// <inheritdoc />
    public async Task<Result<ProfileDto>> HandleAsync(UpdateProfileHeaderCommand command, CancellationToken ct)
    {
        var profile = await repository.GetByIdAsync(userAccessor.UserId, ct);
        if (profile is null)
        {
            return Result<ProfileDto>.Fail("Profile not found.");
        }

        var request = command.Request;
        var now = DateTimeOffset.UtcNow;

        if (request.FirstName != null || request.LastName != null)
        {
            profile.ChangeName(PersonName.From(request.FirstName ?? profile.Name.FirstName, request.LastName ?? profile.Name.LastName), now);
        }

        if (request.Role != null)
        {
            profile.ChangeRole(new RoleTitle(request.Role), now);
        }

        if (request.AvatarUrl != null)
        {
            profile.ChangeAvatar(Avatar.TryFrom(request.AvatarUrl), now);
        }

        if (request.OpenToWork.HasValue)
        {
            profile.SetOpenToWork(request.OpenToWork.Value, now);
        }


        if (request.VerificationStatus != null)
        {
            profile.SetVerified(VerificationStatus.TryFrom(request.VerificationStatus), now);
        }

        var updatedProfile = await repository.UpdateAsync(profile, ct);
        return updatedProfile?.ToDto() ?? Result<ProfileDto>.Fail("Failed to update profile.");
    }
}