using System.Security.Cryptography;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;

namespace CVA.Infrastructure.Storage.S3;

/// <summary>
/// Minimal S3-compatible object storage wrapper: exists/put + presigned URL.
/// </summary>
/// <param name="s3">S3 client.</param>
/// <param name="options">S3 options.</param>
public sealed class S3ObjectStorage(IAmazonS3 s3, IOptions<S3Options> options)
{
    /// <summary>
    /// Checks whether an object exists.
    /// </summary>
    /// <param name="key">Object key.</param>
    /// <param name="ct">Cancellation token.</param>
    public async Task<bool> ExistsAsync(string key, CancellationToken ct)
    {
        try
        {
            await s3.GetObjectMetadataAsync(options.Value.Bucket, key, ct);
            return true;
        }
        catch (AmazonS3Exception e) when (e.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }
    }

    /// <summary>
    /// Uploads an object.
    /// </summary>
    /// <param name="key">Object key.</param>
    /// <param name="content">Content stream.</param>
    /// <param name="contentType">Content type.</param>
    /// <param name="ct">Cancellation token.</param>
    public async Task PutAsync(string key, Stream content, string contentType, CancellationToken ct)
    {
        var request = new PutObjectRequest
        {
            BucketName = options.Value.Bucket,
            Key = key,
            InputStream = content,
            ContentType = contentType
        };

        await s3.PutObjectAsync(request, ct);
    }

    /// <summary>
    /// Builds a presigned GET URL for downloading an object.
    /// </summary>
    /// <param name="key">Object key.</param>
    /// <param name="fileName">Download filename for Content-Disposition.</param>
    /// <param name="ttl">URL time-to-live.</param>
    public Uri GetPresignedDownloadUrl(string key, string fileName, TimeSpan ttl)
    {
        var overrides = new ResponseHeaderOverrides
        {
            ContentDisposition = $"attachment; filename=\"{fileName}\""
        };
        var req = new GetPreSignedUrlRequest
        {
            BucketName = options.Value.Bucket,
            Key = key,
            Verb = HttpVerb.GET,
            Expires = DateTime.UtcNow.Add(ttl),
            ResponseHeaderOverrides = overrides,
        };

        var url = s3.GetPreSignedURL(req);
        return new Uri(url);
    }

    /// <summary>
    /// Computes SHA-256 hex string.
    /// </summary>
    /// <param name="data">Input bytes.</param>
    public static string Sha256Hex(byte[] data)
    {
        var hash = SHA256.HashData(data);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}