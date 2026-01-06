namespace CVA.Infrastructure.Postgres;

/// <summary>
/// EF Core owned type for representing social links.
/// </summary>
internal sealed class SocialLinksEntity
{
    /// <summary>
    /// LinkedIn profile URL.
    /// </summary>
    public string? LinkedIn { get; set; }

    /// <summary>
    /// GitHub profile URL.
    /// </summary>
    public string? GitHub { get; set; }

    /// <summary>
    /// Twitter/X profile URL.
    /// </summary>
    public string? Twitter { get; set; }

    /// <summary>
    /// Telegram profile URL.
    /// </summary>
    public string? Telegram { get; set; }
}