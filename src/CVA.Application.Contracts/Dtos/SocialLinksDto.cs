namespace CVA.Application.Contracts;

/// <summary>
/// Data transfer object that represents social links for a developer profile.
/// </summary>
public sealed record SocialLinksDto
{
    /// <summary>
    /// LinkedIn profile URL.
    /// </summary>
    public string? LinkedIn { get; init; }

    /// <summary>
    /// GitHub profile URL.
    /// </summary>
    public string? GitHub { get; init; }

    /// <summary>
    /// Twitter/X profile URL.
    /// </summary>
    public string? Twitter { get; init; }

    /// <summary>
    /// Telegram profile URL.
    /// </summary>
    public string? Telegram { get; init; }
}