namespace CVA.Application.ProfileService;

/// <summary>
/// Handles the replacement of the entire developer profile.
/// </summary>
/// <param name="repository">The developer profile repository.</param>
/// <param name="userAccessor">The current user accessor.</param>
public sealed class ReplaceProfileHandler(IDeveloperProfileRepository repository, ICurrentUserAccessor userAccessor) 
    : ICommandHandler<ReplaceProfileCommand, DeveloperProfileDto>
{
    /// <inheritdoc />
    public async Task<Result<DeveloperProfileDto>> HandleAsync(ReplaceProfileCommand command, CancellationToken ct)
    {
        var request = command.Request;
        var now = DateTimeOffset.UtcNow;
        var userId = userAccessor.UserId;

        var contactInfo = ContactInfo.Create(
            request.Location?.ToModel(),
            EmailAddress.From(request.Email ?? string.Empty),
            PhoneNumber.TryFrom(request.Phone),
            Url.TryFrom(request.Website));

        var socials = request.SocialLinks.ToModel();

        var profile = DeveloperProfile.Create(
            new DeveloperId(userId),
            PersonName.From(request.FirstName ?? string.Empty, request.LastName ?? string.Empty),
            request.Role != null ? new RoleTitle(request.Role) : null,
            ProfileSummary.TryFrom(request.Summary),
            Avatar.TryFrom(request.AvatarUrl),
            contactInfo,
            socials,
            VerificationStatus.TryFrom(request.VerificationStatus),
            new OpenToWorkStatus(request.OpenToWork),
            now);

        profile.ReplaceSkills(request.Skills.Select(SkillTag.From), now);

        foreach (var dto in request.Projects)
        {
            var (name, description, link, icon, techStack) = dto.ToDomain();
            profile.AddProject(name, description, icon, link, techStack, now);
        }

        foreach (var dto in request.WorkExperience)
        {
            var (company, location, role, description, period, techStack) = dto.ToDomain();
            profile.AddWorkExperience(company, location, role, description, period, techStack, now);
        }

        var updatedProfile = await repository.UpdateAsync(profile, ct);
        return updatedProfile?.ToDto() ?? Result<DeveloperProfileDto>.Fail("Failed to update profile.");
    }
}