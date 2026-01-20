namespace CVA.Infrastructure.Mongo;

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
    /// The email address associated with the user document.
    /// </summary>
    public string Email { get; set; } = null!;

    public string Role { get; set; } = nameof(UserRole.User);

    public string GoogleSubject { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
}