namespace CVA.Application.Contracts;

/// <summary>
/// Represents a request to create a new developer profile for the current authenticated user.
/// </summary>
public sealed record CreateProfileRequest
{
    /// <summary>
    /// The developer's first name.
    /// </summary>
    public required string FirstName { get; init; }

    /// <summary>
    /// The developer's last name.
    /// </summary>
    public required string LastName { get; init; }

    /// <summary>
    /// The developer's email address.
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// The developer's primary role or position.
    /// </summary>
    public string? Role { get; init; }

    /// <summary>
    /// A short profile summary.
    /// </summary>
    public string? Summary { get; init; }

    /// <summary>
    /// The URL to the developer avatar image.
    /// </summary>
    public string? AvatarUrl { get; init; }

    /// <summary>
    /// Indicates whether the developer is open to work.
    /// </summary>
    public bool OpenToWork { get; init; }

    /// <summary>
    /// The number of years of professional experience.
    /// </summary>
    public int YearsOfExperience { get; init; }

    /// <summary>
    /// The developer location (city + country).
    /// </summary>
    public LocationDto? Location { get; init; }

    /// <summary>
    /// Optional phone number.
    /// </summary>
    public string? Phone { get; init; }

    /// <summary>
    /// Optional website URL.
    /// </summary>
    public string? Website { get; init; }

    /// <summary>
    /// Social links.
    /// </summary>
    public SocialLinksDto SocialLinks { get; init; } = new();
}