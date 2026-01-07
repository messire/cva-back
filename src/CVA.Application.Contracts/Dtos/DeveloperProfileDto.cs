using CVA.Domain.Models;

namespace CVA.Application.Contracts;

/// <summary>
/// Extended developer profile DTO. Does not replace UserDto.
/// </summary>
public sealed record DeveloperProfileDto
{
    /// <summary>
    /// Profile identifier.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Developer role or title.
    /// </summary>
    public string? Role { get; init; }

    /// <summary>
    /// Profile summary.
    /// </summary>
    public string? Summary { get; init; }

    /// <summary>
    /// Avatar image URL.
    /// </summary>
    public string? AvatarUrl { get; init; }

    /// <summary>
    /// Indicates whether the developer is open to work.
    /// </summary>
    public bool OpenToWork { get; init; }

    /// <summary>
    /// Indicates whether the profile is verified.
    /// </summary>
    public VerificationLevel? Verified { get; init; }

    /// <summary>
    /// Years of professional experience.
    /// </summary>
    public int YearsOfExperience { get; init; }

    /// <summary>
    /// Contact location.
    /// </summary>
    public LocationDto? Location { get; init; }

    /// <summary>
    /// Social links.
    /// </summary>
    public SocialLinksDto? SocialLinks { get; init; }

    /// <summary>
    /// Skill tags.
    /// </summary>
    public string[] Skills { get; init; } = [];

    /// <summary>
    /// Projects.
    /// </summary>
    public ProjectDto[] Projects { get; init; } = [];

    /// <summary>
    /// Work experience entries.
    /// </summary>
    public WorkExperienceDto[] WorkExperience { get; init; } = [];
}