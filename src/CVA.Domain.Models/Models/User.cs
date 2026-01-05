namespace CVA.Domain.Models;

/// <summary>
/// Represents a user aggregate root.
/// Purpose: enforce invariants and provide behavior for managing user profile and work experience.
/// </summary>
public sealed partial class User : AggregateRoot
{
    private readonly List<Work> _workExperience = [];
    private readonly List<string> _skills = [];

    /// <summary>
    /// Unique identifier of the user.
    /// </summary>
    public Guid Id { get; init; } = Guid.CreateVersion7();

    /// <summary>
    /// User first name.
    /// </summary>
    public string Name { get; private set; } = null!;

    /// <summary>
    /// User last name.
    /// </summary>
    public string Surname { get; private set; } = null!;

    /// <summary>
    /// User email.
    /// </summary>
    public EmailObject Email { get; private set; }

    /// <summary>
    /// Optional phone number.
    /// </summary>
    public string? Phone { get; private set; }

    /// <summary>
    /// Optional URL of the user's profile picture.
    /// </summary>
    public string? Photo { get; private set; }

    /// <summary>
    /// Optional birthday.
    /// </summary>
    public DateOnly? Birthday { get; private set; }

    /// <summary>
    /// Optional user summary.
    /// </summary>
    public string? SummaryInfo { get; private set; }

    /// <summary>
    /// Skills list (read-only view).
    /// </summary>
    public IReadOnlyList<string> Skills => _skills;

    /// <summary>
    /// Work experience list (read-only view).
    /// </summary>
    public IReadOnlyCollection<Work> WorkExperience => _workExperience;

    /// <summary>
    /// EF/Mongo constructor.
    /// </summary>
    private User()
    {
    }

    /// <summary>
    /// Rehydrate aggregate from persistence.
    /// </summary>
    /// <param name="id">Unique identifier of the user.</param>
    /// <param name="name">User first name.</param>
    /// <param name="surname">User last name.</param>
    /// <param name="email">User email.</param>
    private User(Guid id, string name, string surname, string email)
    {
        if (id == Guid.Empty)
            throw new DomainValidationException("Id must not be empty.");

        Id = id;
        Name = RequireNonEmpty(name, nameof(name));
        Surname = RequireNonEmpty(surname, nameof(surname));
        Email = EmailObject.Create(email);
    }
}