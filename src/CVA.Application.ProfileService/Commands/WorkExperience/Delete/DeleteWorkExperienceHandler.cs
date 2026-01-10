namespace CVA.Application.ProfileService;

/// <summary>
/// Handles the deletion of a work experience entry from the developer profile.
/// </summary>
/// <param name="repository">The developer profile repository.</param>
/// <param name="userAccessor">The current user accessor.</param>
public sealed class DeleteWorkExperienceHandler(IDeveloperProfileRepository repository, ICurrentUserAccessor userAccessor) 
    : ICommandHandler<DeleteWorkExperienceCommand, DeveloperProfileDto>
{
    /// <inheritdoc />
    public async Task<Result<DeveloperProfileDto>> HandleAsync(DeleteWorkExperienceCommand command, CancellationToken ct)
    {
        var profile = await repository.GetByIdAsync(userAccessor.UserId, ct);
        if (profile is null)
        {
            return Result<DeveloperProfileDto>.Fail("Profile not found.");
        }

        var workId = new WorkExperienceId(command.WorkExperienceId);
        if (profile.WorkExperience.All(item => item.Id != workId))
        {
            return Result<DeveloperProfileDto>.Fail("Work experience not found.");
        }

        profile.RemoveWorkExperience(workId, DateTimeOffset.UtcNow);
        var updatedProfile = await repository.UpdateAsync(profile, ct);
        return updatedProfile?.ToDto() ?? Result<DeveloperProfileDto>.Fail("Failed to update profile.");
    }
}