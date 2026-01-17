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
    /// Frontend base URL.
    /// </summary>
    public string FrontendBaseUrl { get; set; } = string.Empty;

    /// <summary>
    /// Profile page path template.
    /// Supported placeholders: {id}.
    /// </summary>
    public string ProfilePathTemplate { get; set; } = "/profile/{id}?print=1";

    /// <summary>
    /// CSS selector that must exist on the profile page before generating the PDF.
    /// Prevents silently generating the wrong page (e.g. catalog/home).
    /// </summary>
    public string ProfileReadySelector { get; set; } = "[data-print-anchor=\"profile-page\"]";

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