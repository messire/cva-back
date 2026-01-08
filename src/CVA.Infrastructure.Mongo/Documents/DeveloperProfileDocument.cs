using MongoDB.Bson.Serialization.Attributes;

namespace CVA.Infrastructure.Mongo;

/// <summary>
/// Represents a MongoDB document for a developer profile aggregate.
/// </summary>
internal sealed class DeveloperProfileDocument
{
    /// <summary>
    /// The unique identifier for the developer profile document.
    /// </summary>
    [BsonId]
    public Guid Id { get; set; }

    /// <summary>
    /// The first name of the developer profile owner.
    /// </summary>
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// The last name of the developer profile owner.
    /// </summary>
    public string LastName { get; set; } = null!;

    /// <summary>
    /// The role of the developer profile owner.
    /// </summary>
    public string? Role { get; set; }

    /// <summary>
    /// Developer phone number.
    /// </summary>
    public string? Phone { get; init; }

    /// <summary>
    /// The summary of the developer profile owner.
    /// </summary>
    public string? Summary { get; set; }

    /// <summary>
    /// The URL of the developer profile owner's avatar.
    /// </summary>
    public string? AvatarUrl { get; set; }

    /// <summary>
    /// Indicates if the developer profile owner is open to work.
    /// </summary>
    public bool OpenToWork { get; set; }

    /// <summary>
    /// The years of experience of the developer profile owner.
    /// </summary>
    public int YearsOfExperience { get; set; }

    /// <summary>
    /// The verification status of the developer profile owner.
    /// </summary>
    public int VerificationStatus { get; set; }

    /// <summary>
    /// The email of the developer profile owner.
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// The website of the developer profile owner.
    /// </summary>
    public string? Website { get; set; }

    /// <summary>
    /// The location of the developer profile owner.
    /// </summary>
    public LocationDocument? Location { get; set; }

    /// <summary>
    /// The social links of the developer profile owner.
    /// </summary>
    public SocialLinksDocument? SocialLinks { get; set; }

    /// <summary>
    /// The skills of the developer profile owner.
    /// </summary>
    public List<string> Skills { get; set; } = [];

    /// <summary>
    /// The achievements of the developer profile owner.
    /// </summary>
    public List<ProjectDocument> Projects { get; set; } = [];

    /// <summary>
    /// The work experience of the developer profile owner.
    /// </summary>
    public List<WorkExperienceDocument> WorkExperience { get; set; } = [];

    /// <summary>
    /// The date and time when the developer profile was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// The date and time when the developer profile was last updated.
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; }
}