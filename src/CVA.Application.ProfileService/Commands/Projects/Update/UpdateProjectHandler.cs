namespace CVA.Application.ProfileService;

/// <summary>
/// Handles the update of an existing project in the developer profile.
/// </summary>
/// <param name="repository">The developer profile repository.</param>
/// <param name="userAccessor">The current user accessor.</param>
public sealed class UpdateProjectHandler(IDeveloperProfileRepository repository, ICurrentUserAccessor userAccessor) 
    : ICommandHandler<UpdateProjectCommand, ProfileDto>
{
    /// <inheritdoc />
    public async Task<Result<ProfileDto>> HandleAsync(UpdateProjectCommand command, CancellationToken ct)
    {
        var profile = await repository.GetByIdAsync(userAccessor.UserId, ct);
        if (profile is null)
        {
            return Result<ProfileDto>.Fail("Profile not found.");
        }

        var request = command.Request;
        var now = DateTimeOffset.UtcNow;
        var projectId = new ProjectId(command.ProjectId);

        if (profile.Projects.All(item => item.Id != projectId))
        {
            return Result<ProfileDto>.Fail("Project not found.");
        }

        var (name, description, link, icon, techStack) = request.ToDomain();
        profile.UpdateProject(projectId, name, description, icon, link, techStack, now);
        var updatedProfile = await repository.UpdateAsync(profile, ct);
        return updatedProfile?.ToDto() ?? Result<ProfileDto>.Fail("Failed to update profile.");
    }
}