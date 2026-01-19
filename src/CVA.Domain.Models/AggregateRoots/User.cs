namespace CVA.Domain.Models;

/// <summary>
/// Represents a user aggregate root.
/// Purpose: enforce invariants and provide behavior for managing user profile and work experience.
/// </summary>
public sealed class User : AggregateRoot
{
    /// <summary>
    /// Unique identifier of the user.
    /// </summary>
    public Guid Id { get; init; } = Guid.CreateVersion7();

    /// <summary>
    /// User email.
    /// </summary>
    public EmailAddress Email { get; private set; }

    /// <summary>
    /// User role stored in the database.
    /// </summary>
    public UserRole Role { get; private set; } = UserRole.User;

    /// <summary>
    /// Google subject identifier (<c>sub</c> claim) for the linked external login.
    /// </summary>
    public string GoogleSubject { get; private set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class.
    /// </summary>
    private User()
    {
    }

    /// <summary>
    /// Rehydrate aggregate from persistence.
    /// </summary>
    private User(Guid id, string email, string googleSubject, UserRole role, DateTimeOffset createdAt, DateTimeOffset updatedAt)
    {
        Ensure.NotNull(googleSubject, nameof(googleSubject));
        Ensure.NotEmpty(id, nameof(id));

        Id = id;
        Email = EmailAddress.From(email);
        GoogleSubject = googleSubject;
        Role = role;
        CreatedAt = createdAt;
        Touch(updatedAt);
    }

    /// <summary>
    /// Creates a new user account linked to Google external login.
    /// </summary>
    /// <param name="email">User email.</param>
    /// <param name="googleSubject">Google subject identifier (<c>sub</c> claim).</param>
    /// <param name="role">Initial role.</param>
    public static User CreateFromGoogle(string email, string googleSubject, UserRole role)
        => new(
            id: Guid.CreateVersion7(),
            email: email,
            googleSubject: googleSubject,
            role: role,
            createdAt: DateTime.UtcNow,
            updatedAt: DateTime.UtcNow);

    /// <summary>
    /// Restores a user from persistence.
    /// </summary>
    /// <param name="id">User id.</param>
    /// <param name="email">Email.</param>
    /// <param name="googleSubject">Google subject.</param>
    /// <param name="role">Role.</param>
    /// <param name="createdAt">Created at (UTC).</param>
    /// <param name="updatedAt">Updated at (UTC).</param>
    public static User FromPersistence(Guid id, string email, string googleSubject, UserRole role, DateTimeOffset createdAt, DateTimeOffset updatedAt)
        => new(id, email, googleSubject, role, createdAt, updatedAt);

    /// <summary>
    /// Updates the user role.
    /// </summary>
    /// <param name="role">New role.</param>
    public void ChangeRole(UserRole role)
    {
        Role = role;
        Touch(DateTime.UtcNow);
    }
}