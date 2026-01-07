using CVA.Domain.Models;

namespace CVA.Application.DeveloperProfiles;

/// <summary>
/// Provides extension methods for mapping domain models to DTOs.
/// </summary>
public static class DeveloperProfileMapping
{
    /// <summary>
    /// Maps a <see cref="DeveloperProfile"/> domain model to a <see cref="DeveloperProfileDto"/>.
    /// </summary>
    public static DeveloperProfileDto ToDto(this DeveloperProfile profile)
        => new()
        {
            Id = profile.Id.Value,
            Role = profile.Role?.Value,
            Summary = profile.Summary?.Value,
            AvatarUrl = profile.Avatar?.ImageUrl.Value,
            OpenToWork = profile.OpenToWork.Value,
            Verified = profile.Verification.Value,
            YearsOfExperience = profile.YearsOfExperience.Value,
            Location = profile.Contact.Location?.ToDto(),
            SocialLinks = profile.Social.ToDto(),
            Skills = profile.Skills.Select(tag => tag.Value).ToArray(),
            Projects = profile.Projects.Select(item => item.ToDto()).ToArray(),
            WorkExperience = profile.WorkExperience.Select(item => item.ToDto()).ToArray()
        };

    /// <summary>
    /// Maps a <see cref="DeveloperProfile"/> domain model to a <see cref="DeveloperProfileCardDto"/>.
    /// </summary>
    public static DeveloperProfileCardDto ToCardDto(this DeveloperProfile profile)
        => new()
        {
            Id = profile.Id.Value,
            FirstName = profile.Name.FirstName,
            LastName = profile.Name.LastName,
            Role = profile.Role?.Value,
            AvatarUrl = profile.Avatar?.ImageUrl.Value,
            OpenToWork = profile.OpenToWork.Value,
            VerificationStatus = profile.Verification.Value.ToString(),
            Skills = profile.Skills.Select(tag => tag.Value).ToArray()
        };

    /// <summary>
    /// Maps a <see cref="Location"/> domain model to a <see cref="LocationDto"/>.
    /// </summary>
    private static LocationDto ToDto(this Location location)
        => new()
        {
            City = location.City,
            Country = location.Country
        };

    /// <summary>
    /// Maps a <see cref="ProjectItem"/> domain model to a <see cref="ProjectDto"/>.
    /// </summary>
    private static ProjectDto ToDto(this ProjectItem project)
        => new()
        {
            Id = project.Id.Value,
            Name = project.Name.Value,
            Description = project.Description?.Value,
            LinkUrl = project.Link.Value?.Value,
            IconUrl = project.Icon?.ImageUrl.Value,
            TechStack = project.TechStack.Select(tag => tag.Value).ToArray()
        };

    /// <summary>
    /// Maps a <see cref="WorkExperienceItem"/> domain model to a <see cref="WorkExperienceDto"/>.
    /// </summary>
    private static WorkExperienceDto ToDto(this WorkExperienceItem work)
        => new()
        {
            Id = work.Id.Value,
            Company = work.Company.Value,
            Location = work.Location?.ToDto(),
            Role = work.Role?.Value ?? string.Empty,
            Description = work.Description?.Value,
            StartDate = work.Period.Start,
            EndDate = work.Period.End,
            TechStack = work.TechStack.Select(tag => tag.Value).ToArray()
        };

    /// <summary>
    /// Maps a <see cref="SocialLinks"/> domain model to a <see cref="SocialLinksDto"/>.
    /// </summary>
    private static SocialLinksDto ToDto(this SocialLinks social)
        => new()
        {
            LinkedIn = social.LinkedIn?.Value,
            GitHub = social.GitHub?.Value,
            Telegram = social.Telegram?.Value,
            Twitter = social.Twitter?.Value
        };
}