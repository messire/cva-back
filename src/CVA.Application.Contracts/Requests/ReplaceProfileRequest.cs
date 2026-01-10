namespace CVA.Application.Contracts;

/// <summary>
/// Represents a request to replace a developer's profile information.
/// </summary>
public sealed record ReplaceProfileRequest
{
    /// <summary>
    /// The first name of the developer.
    /// </summary>
    public string? FirstName { get; init; }

    /// <summary>
    /// The last name of the developer.
    /// </summary>
    public string? LastName { get; init; }

    /// <summary>
    /// The primary role or position of the developer.
    /// </summary>
    public string? Role { get; init; }

    /// <summary>
    /// A brief summary or description of the developer's skills and experience.
    /// </summary>
    public string? Summary { get; init; }

    /// <summary>
    /// The URL to the developer's avatar image.
    /// </summary>
    public string? AvatarUrl { get; init; }

    /// <summary>
    /// Indicates whether the developer is open to new work opportunities.
    /// </summary>
    public bool OpenToWork { get; init; }

    /// <summary>
    /// The current verification status of the developer's profile.
    /// </summary>
    public string? VerificationStatus { get; init; }

    /// <summary>
    /// The number of years of professional experience the developer has.
    /// </summary>
    public int YearsOfExperience { get; init; }

    /// <summary>
    /// The developer's email address.
    /// </summary>
    public string? Email { get; init; }

    /// <summary>
    /// The developer's phone number.
    /// </summary>
    public string? Phone { get; init; }

    /// <summary>
    /// The developer's website or portfolio URL.
    /// </summary>
    public string? Website { get; init; }

    /// <summary>
    /// The developer's location.
    /// </summary>
    public LocationDto? Location { get; init; }

    /// <summary>
    /// Represents the social media links associated with the developer's profile.
    /// </summary>
    public SocialLinksDto? SocialLinks { get; init; }

    /// <summary>
    /// The developer's skills and expertise.
    /// </summary>
    public string[] Skills { get; init; } = [];

    /// <summary>
    /// The developer's projects and accomplishments.
    /// </summary>
    public ProjectDto[] Projects { get; init; } = [];

    /// <summary>
    /// The developer's work experience history.
    /// </summary>
    public WorkExperienceDto[] WorkExperience { get; init; } = [];
}