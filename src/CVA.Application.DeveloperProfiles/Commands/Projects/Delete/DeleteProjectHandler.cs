namespace CVA.Application.DeveloperProfiles;

/// <summary>
/// Handles the deletion of a project from the developer profile.
/// </summary>
/// <param name="repository">The developer profile repository.</param>
/// <param name="userAccessor">The current user accessor.</param>
public sealed class DeleteProjectHandler(IDeveloperProfileRepository repository, ICurrentUserAccessor userAccessor)
    : ICommandHandler<DeleteProjectCommand, DeveloperProfileDto>
{
    /// <inheritdoc />
    public async Task<Result<DeveloperProfileDto>> HandleAsync(DeleteProjectCommand command, CancellationToken ct)
    {
        var profile = await repository.GetByIdAsync(userAccessor.UserId, ct);
        if (profile is null)
        {
            return Result<DeveloperProfileDto>.Fail("Profile not found.");
        }

        var projectId = new ProjectId(command.ProjectId);
        if (profile.Projects.All(item => item.Id != projectId))
        {
            return Result<DeveloperProfileDto>.Fail("Project not found.");
        }

        profile.RemoveProject(projectId, DateTimeOffset.UtcNow);
        var updatedProfile = await repository.UpdateAsync(profile, ct);
        return updatedProfile?.ToDto() ?? Result<DeveloperProfileDto>.Fail("Failed to update profile.");
    }
}