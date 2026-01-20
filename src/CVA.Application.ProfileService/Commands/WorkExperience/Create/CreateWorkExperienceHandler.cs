namespace CVA.Application.ProfileService;

/// <summary>
/// Handles the creation of a new work experience entry in the developer profile.
/// </summary>
/// <param name="repository">The developer profile repository.</param>
/// <param name="userAccessor">The current user accessor.</param>
public sealed class CreateWorkExperienceHandler(IDeveloperProfileRepository repository, ICurrentUserAccessor userAccessor) 
    : ICommandHandler<CreateWorkExperienceCommand, ProfileDto>
{
    /// <inheritdoc />
    public async Task<Result<ProfileDto>> HandleAsync(CreateWorkExperienceCommand command, CancellationToken ct)
    {
        var profile = await repository.GetByIdAsync(userAccessor.UserId, ct);
        if (profile is null)
        {
            return Result<ProfileDto>.Fail("Profile not found.");
        }

        var request = command.Request;
        var now = DateTimeOffset.UtcNow;

        var (company, location, role, description, period, techStack) = request.ToDomain();
        profile.AddWorkExperience(company, location, role, description, period, techStack, now);

        var updatedProfile = await repository.UpdateAsync(profile, ct);
        return updatedProfile?.ToDto() ?? Result<ProfileDto>.Fail("Failed to update profile.");
    }
}