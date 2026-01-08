namespace CVA.Infrastructure.Postgres;

/// <summary>
/// EF Core persistence model for DeveloperProfile.
/// </summary>
internal sealed class DeveloperProfileEntity
{
    /// <summary>
    /// Unique identifier of the developer profile.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Developer first name.
    /// </summary>
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// Developer last name.
    /// </summary>
    public string LastName { get; set; } = null!;

    /// <summary>
    /// Developer role/title.
    /// </summary>
    public string? Role { get; set; }

    /// <summary>
    /// Profile summary.
    /// </summary>
    public string? Summary { get; set; }

    /// <summary>
    /// Avatar image URL.
    /// </summary>
    public string? AvatarUrl { get; set; }

    /// <summary>
    /// Indicates whether the developer is open to work.
    /// </summary>
    public bool OpenToWork { get; set; }

    /// <summary>
    /// Total years of professional experience.
    /// </summary>
    public int YearsOfExperience { get; set; }

    /// <summary>
    /// Contact email.
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Contact phone number.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Contact website URL.
    /// </summary>
    public string? Website { get; set; }

    /// <summary>
    /// Indicates whether the profile is verified.
    /// </summary>
    public VerificationLevel Verified { get; set; }

    /// <summary>
    /// Structured location.
    /// </summary>
    public LocationEntity? Location { get; set; }

    /// <summary>
    /// Social links.
    /// </summary>
    public SocialLinksEntity? SocialLinks { get; set; }

    /// <summary>
    /// Skill tags.
    /// </summary>
    public List<string> Skills { get; set; } = [];

    /// <summary>
    /// Projects list.
    /// </summary>
    public List<ProjectEntity> Projects { get; set; } = [];

    /// <summary>
    /// Work experience list.
    /// </summary>
    public List<WorkExperienceEntity> WorkExperience { get; set; } = [];

    /// <summary>
    /// Created timestamp.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Updated timestamp.
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; }
}