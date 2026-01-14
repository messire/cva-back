namespace CVA.Infrastructure.Common.Media;

/// <summary>
/// Configuration options for media storage.
/// </summary>
public sealed class MediaOptions
{
    /// <summary>
    /// Configuration section name.
    /// </summary>
    public const string Path = "Media";

    /// <summary>
    /// Physical root directory where media files are stored.
    /// </summary>
    public string RootPath { get; init; } = string.Empty;

    /// <summary>
    /// Public request path mapped to the media root.
    /// </summary>
    public string PublicRequestPath { get; init; } = "/media";
}