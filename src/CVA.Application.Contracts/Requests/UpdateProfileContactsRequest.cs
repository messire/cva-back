namespace CVA.Application.Contracts;

/// <summary>
/// Represents a request to update a developer's profile contacts.
/// </summary>
public sealed record UpdateProfileContactsRequest
{
    /// <summary>
    /// The developer's email address.
    /// </summary>
    public string? Email { get; init; }

    /// <summary>
    /// The developer's phone number.
    /// </summary>
    public string? Phone { get; init; }

    /// <summary>
    /// The developer's website or portfolio URL.
    /// </summary>
    public string? Website { get; init; }

    /// <summary>
    /// The developer's location.
    /// </summary>
    public LocationDto? Location { get; init; }

    /// <summary>
    /// The developer's social media links.
    /// </summary>
    public SocialLinksDto? SocialLinks { get; init; }
}