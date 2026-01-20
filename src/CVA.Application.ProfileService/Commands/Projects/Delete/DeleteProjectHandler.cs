namespace CVA.Application.ProfileService;

/// <summary>
/// Handles the deletion of a project from the developer profile.
/// </summary>
/// <param name="repository">The developer profile repository.</param>
/// <param name="userAccessor">The current user accessor.</param>
public sealed class DeleteProjectHandler(IDeveloperProfileRepository repository, ICurrentUserAccessor userAccessor)
    : ICommandHandler<DeleteProjectCommand, ProfileDto>
{
    /// <inheritdoc />
    public async Task<Result<ProfileDto>> HandleAsync(DeleteProjectCommand command, CancellationToken ct)
    {
        var profile = await repository.GetByIdAsync(userAccessor.UserId, ct);
        if (profile is null)
        {
            return Result<ProfileDto>.Fail("Profile not found.");
        }

        var projectId = new ProjectId(command.ProjectId);
        if (profile.Projects.All(item => item.Id != projectId))
        {
            return Result<ProfileDto>.Fail("Project not found.");
        }

        profile.RemoveProject(projectId, DateTimeOffset.UtcNow);
        var updatedProfile = await repository.UpdateAsync(profile, ct);
        return updatedProfile?.ToDto() ?? Result<ProfileDto>.Fail("Failed to update profile.");
    }
}