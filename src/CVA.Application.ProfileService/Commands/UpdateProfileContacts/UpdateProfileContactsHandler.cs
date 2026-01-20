namespace CVA.Application.ProfileService;

/// <summary>
/// Handles the update of contact information in the developer profile.
/// </summary>
/// <param name="repository">The developer profile repository.</param>
/// <param name="userAccessor">The current user accessor.</param>
public sealed class UpdateProfileContactsHandler(IDeveloperProfileRepository repository, ICurrentUserAccessor userAccessor)
    : ICommandHandler<UpdateProfileContactsCommand, ProfileDto>
{
    /// <inheritdoc />
    public async Task<Result<ProfileDto>> HandleAsync(UpdateProfileContactsCommand command, CancellationToken ct)
    {
        var profile = await repository.GetByIdAsync(userAccessor.UserId, ct);
        if (profile is null)
        {
            return Result<ProfileDto>.Fail("Profile not found.");
        }

        var request = command.Request;
        var now = DateTimeOffset.UtcNow;
        profile.ChangeContact(request.ToModel(profile.Contact), now);
        profile.ChangeSocialLinks(request.SocialLinks.ToModel(), now);
        var updatedProfile = await repository.UpdateAsync(profile, ct);
        return updatedProfile?.ToDto() ?? Result<ProfileDto>.Fail("Failed to update profile.");
    }
}