namespace CVA.Application.DeveloperProfiles;

/// <summary>
/// Handles the update of an existing work experience entry in the developer profile.
/// </summary>
/// <param name="repository">The developer profile repository.</param>
/// <param name="userAccessor">The current user accessor.</param>
public sealed class UpdateWorkExperienceHandler(
    IDeveloperProfileRepository repository,
    ICurrentUserAccessor userAccessor) 
    : ICommandHandler<UpdateWorkExperienceCommand, DeveloperProfileDto>
{
    /// <inheritdoc />
    public async Task<Result<DeveloperProfileDto>> HandleAsync(UpdateWorkExperienceCommand command, CancellationToken ct)
    {
        var profile = await repository.GetByIdAsync(userAccessor.UserId, ct);
        if (profile is null)
        {
            return Result<DeveloperProfileDto>.Fail("Profile not found.");
        }

        var request = command.Request;
        var now = DateTimeOffset.UtcNow;
        var (company, location, role, description, period, techStack) = request.ToDomain();
        profile.UpdateWorkExperience(new WorkExperienceId(command.WorkExperienceId), company, location, role, description, period, techStack, now);
        var updatedProfile = await repository.UpdateAsync(profile, ct);
        return updatedProfile?.ToDto() ?? Result<DeveloperProfileDto>.Fail("Failed to update profile.");
    }
}