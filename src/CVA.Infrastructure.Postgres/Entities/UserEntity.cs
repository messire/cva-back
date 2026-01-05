namespace CVA.Infrastructure.Postgres;

/// <summary>
/// EF Core persistence model for User.
/// </summary>
internal sealed class UserEntity
{
    /// <summary>
    /// Unique identifier for the user entity, stored in the database.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The name of the user.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// The family name or last name of the user.
    /// </summary>
    public string Surname { get; set; } = null!;

    /// <summary>
    /// The email address associated with the user.
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// The phone number associated with the user.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// The URL of the user's profile picture.
    /// </summary>
    public string? Photo { get; set; }

    /// <summary>
    /// The date of birth of the user.
    /// </summary>
    public DateOnly? Birthday { get; set; }

    /// <summary>
    /// A brief summary or description of the user's profile or background.'
    /// </summary>
    public string? SummaryInfo { get; set; }

    /// <summary>
    /// A collection of skills associated with the user.
    /// </summary>
    public List<string> Skills { get; set; } = [];

    /// <summary>
    /// Owned collection, stored in table "works".
    /// </summary>
    public List<WorkEntity> WorkExperience { get; set; } = [];
}