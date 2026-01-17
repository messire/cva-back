namespace CVA.Infrastructure.ResumePdf;

/// <summary>
/// Configuration options for resume PDF generation via Playwright.
/// </summary>
public sealed class ResumePdfOptions
{
    /// <summary>
    /// The configuration section path for resume pdf options.
    /// </summary>
    public const string Path = "ResumePdf";

    /// <summary>
    /// Frontend base URL (e.g. https://cva-production.up.railway.app or http://frontend:3000).
    /// </summary>
    public string FrontendBaseUrl { get; set; } = string.Empty;

    /// <summary>
    /// Profile page path template (e.g. /profile/{id}?print=1).
    /// Supported placeholders: {id}.
    /// </summary>
    public string ProfilePathTemplate { get; set; } = "/profile/{id}?print=1";

    /// <summary>
    /// Cache prefix inside the bucket.
    /// </summary>
    public string CachePrefix { get; set; } = "resumes";

    /// <summary>
    /// Presigned URL lifetime in minutes.
    /// </summary>
    public int PresignedUrlTtlMinutes { get; set; } = 60;

    /// <summary>
    /// Navigation timeout in seconds.
    /// </summary>
    public int NavigationTimeoutSeconds { get; set; } = 60;
}