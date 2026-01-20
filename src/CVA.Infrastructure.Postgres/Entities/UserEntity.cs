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
    /// The email address associated with the user.
    /// </summary>
    public string Email { get; set; } = null!;

    public string Role { get; set; } = nameof(UserRole.User);

    public string GoogleSubject { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
}