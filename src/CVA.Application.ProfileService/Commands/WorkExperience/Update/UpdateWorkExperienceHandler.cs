namespace CVA.Application.ProfileService;

/// <summary>
/// Handles the update of an existing work experience entry in the developer profile.
/// </summary>
/// <param name="repository">The developer profile repository.</param>
/// <param name="userAccessor">The current user accessor.</param>
public sealed class UpdateWorkExperienceHandler(
    IDeveloperProfileRepository repository,
    ICurrentUserAccessor userAccessor) 
    : ICommandHandler<UpdateWorkExperienceCommand, ProfileDto>
{
    /// <inheritdoc />
    public async Task<Result<ProfileDto>> HandleAsync(UpdateWorkExperienceCommand command, CancellationToken ct)
    {
        var profile = await repository.GetByIdAsync(userAccessor.UserId, ct);
        if (profile is null)
        {
            return Result<ProfileDto>.Fail("Profile not found.");
        }

        var request = command.Request;
        var now = DateTimeOffset.UtcNow;
        var workId = new WorkExperienceId(command.WorkExperienceId);

        if (profile.WorkExperience.All(item => item.Id != workId))
        {
            return Result<ProfileDto>.Fail("Work experience not found.");
        }

        var (company, location, role, description, period, techStack) = request.ToDomain();
        profile.UpdateWorkExperience(workId, company, location, role, description, period, techStack, now);
        var updatedProfile = await repository.UpdateAsync(profile, ct);
        return updatedProfile?.ToDto() ?? Result<ProfileDto>.Fail("Failed to update profile.");
    }
}