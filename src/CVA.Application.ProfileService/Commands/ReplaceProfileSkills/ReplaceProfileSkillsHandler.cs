namespace CVA.Application.ProfileService;

/// <summary>
/// Handles the replacement of skills in the developer profile.
/// </summary>
/// <param name="repository">The developer profile repository.</param>
/// <param name="userAccessor">The current user accessor.</param>
public sealed class ReplaceProfileSkillsHandler(IDeveloperProfileRepository repository, ICurrentUserAccessor userAccessor) 
    : ICommandHandler<ReplaceProfileSkillsCommand, DeveloperProfileDto>
{
    /// <inheritdoc />
    public async Task<Result<DeveloperProfileDto>> HandleAsync(ReplaceProfileSkillsCommand command, CancellationToken ct)
    {
        var profile = await repository.GetByIdAsync(userAccessor.UserId, ct);
        if (profile is null)
        {
            return Result<DeveloperProfileDto>.Fail("Profile not found.");
        }

        profile.ReplaceSkills(command.Skills.Select(SkillTag.From), DateTimeOffset.UtcNow);
        var updatedProfile = await repository.UpdateAsync(profile, ct);
        return updatedProfile?.ToDto() ?? Result<DeveloperProfileDto>.Fail("Failed to update profile.");
    }
}