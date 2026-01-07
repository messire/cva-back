namespace CVA.Application.Contracts;

/// <summary>
/// Developer profile card DTO.
/// </summary>
public sealed record DeveloperProfileCardDto
{
    /// <summary>
    /// Unique identifier for the developer profile card.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// The first name of the developer associated with the profile card.
    /// </summary>
    public string? FirstName { get; init; }

    /// <summary>
    /// The last name of the developer associated with the profile card.
    /// </summary>
    public string? LastName { get; init; }

    /// <summary>
    /// The role or primary responsibility of the developer within a professional context.
    /// </summary>
    public string? Role { get; init; }

    /// <summary>
    /// URL to the avatar image associated with the developer profile.
    /// </summary>
    public string? AvatarUrl { get; init; }

    /// <summary>
    /// Indicates whether the developer is actively seeking new work opportunities.
    /// </summary>
    public bool OpenToWork { get; init; }

    /// <summary>
    /// Indicates the current verification status of the developer profile.
    /// </summary>
    public string? VerificationStatus { get; init; }

    /// <summary>
    /// Represents a collection of skills associated with the developer.
    /// </summary>
    public string[] Skills { get; init; } = [];
}