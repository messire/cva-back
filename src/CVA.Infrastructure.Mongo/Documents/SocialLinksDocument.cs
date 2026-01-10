namespace CVA.Infrastructure.Mongo;

/// <summary>
/// Represents a MongoDB embedded document for social links.
/// </summary>
internal sealed class SocialLinksDocument
{
    /// <summary>
    /// The LinkedIn profile URL.
    /// </summary>
    public string? LinkedIn { get; set; }

    /// <summary>
    /// The GitHub profile URL.
    /// </summary>
    public string? GitHub { get; set; }

    /// <summary>
    /// The Twitter profile URL.
    /// </summary>
    public string? Twitter { get; set; }

    /// <summary>
    /// The Telegram profile URL.
    /// </summary>
    public string? Telegram { get; set; }
}