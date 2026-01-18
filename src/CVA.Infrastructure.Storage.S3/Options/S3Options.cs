namespace CVA.Infrastructure.Storage.S3;

/// <summary>
/// Represents configuration options for an S3-compatible object storage.
/// Works with Railway Bucket (S3-compatible) and local MinIO.
/// </summary>
public sealed class S3Options
{
    /// <summary>
    /// The configuration section path for S3 options.
    /// </summary>
    public const string Path = "S3";

    /// <summary>
    /// Bucket name.
    /// </summary>
    public string Bucket { get; set; } = string.Empty;

    /// <summary>
    /// S3 endpoint URL.
    /// </summary>
    public string Endpoint { get; set; } = string.Empty;

    /// <summary>
    /// Access key id.
    /// </summary>
    public string AccessKeyId { get; set; } = string.Empty;

    /// <summary>
    /// Secret access key.
    /// </summary>
    public string SecretAccessKey { get; set; } = string.Empty;

    /// <summary>
    /// Region name. For some S3-compatible providers can be "auto".
    /// </summary>
    public string Region { get; set; } = "auto";

    /// <summary>
    /// Forces path-style addressing. Usually required for local MinIO.
    /// </summary>
    public bool ForcePathStyle { get; set; } = true;
}