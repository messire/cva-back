namespace CVA.Application.Contracts;

/// <summary>
/// Represents a request to update a developer's profile header information.
/// </summary>
public sealed record UpdateProfileHeaderRequest
{
    /// <summary>
    /// The developer's first name.
    /// </summary>
    public string? FirstName { get; init; }

    /// <summary>
    /// The developer's last name.
    /// </summary>
    public string? LastName { get; init; }

    /// <summary>
    /// The developer's primary role or position.
    /// </summary>
    public string? Role { get; init; }

    /// <summary>
    /// The URL to the developer's avatar image.
    /// </summary>
    public string? AvatarUrl { get; init; }

    /// <summary>
    /// Indicates if the developer is open to work.
    /// </summary>
    public bool? OpenToWork { get; init; }

    /// <summary>
    /// The verification status of the developer's profile.
    /// </summary>
    public string? VerificationStatus { get; init; }

    /// <summary>
    /// The number of years of experience the developer has.
    /// </summary>
    public int? YearsOfExperience { get; init; }

    /// <summary>
    /// The average rating of the developer's profile.
    /// </summary>
    public decimal? Rating { get; init; }
}