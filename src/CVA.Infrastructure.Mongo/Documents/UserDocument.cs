namespace CVA.Infrastructure.Mongo.Documents;

/// <summary>
/// Represents a MongoDB document for a user entity.
/// </summary>
internal sealed class UserDocument
{
    /// <summary>
    /// The unique identifier for the user document.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The name of the user.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// The family name or last name of the user entity.
    /// </summary>
    public string Surname { get; set; } = null!;

    /// <summary>
    /// The email address associated with the user document.
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// The phone number associated with the user.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// The URL of the user's profile picture.
    /// </summary>
    public string? Photo { get; init; }

    /// <summary>
    /// The date of birth of the user.
    /// </summary>
    public DateOnly? Birthday { get; set; }

    /// <summary>
    /// A brief description or personal summary about the user.
    /// </summary>
    public string? SummaryInfo { get; set; }

    /// <summary>
    /// A collection of skills associated with the user.
    /// </summary>
    public List<string> Skills { get; set; } = [];

    /// <summary>
    /// A collection of work experiences associated with the user.
    /// </summary>
    public List<WorkDocument> WorkExperience { get; set; } = [];
}