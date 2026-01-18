namespace CVA.Application.ProfileService;

/// <summary>
/// Handles the creation of a new project in the developer profile.
/// </summary>
/// <param name="repository">The developer profile repository.</param>
/// <param name="userAccessor">The current user accessor.</param>
public sealed class CreateProjectHandler(IDeveloperProfileRepository repository, ICurrentUserAccessor userAccessor)
    : ICommandHandler<CreateProjectCommand, ProfileDto>
{
    /// <inheritdoc />
    public async Task<Result<ProfileDto>> HandleAsync(CreateProjectCommand command, CancellationToken ct)
    {
        var profile = await repository.GetByIdAsync(userAccessor.UserId, ct);
        if (profile is null)
        {
            return Result<ProfileDto>.Fail("Profile not found.");
        }

        var request = command.Request;
        var now = DateTimeOffset.UtcNow;

        var (name, description, link, icon, techStack) = request.ToDomain();
        profile.AddProject(name, description, icon, link, techStack, now);

        var updatedProfile = await repository.UpdateAsync(profile, ct);
        return updatedProfile?.ToDto() ?? Result<ProfileDto>.Fail("Failed to update profile.");
    }
}