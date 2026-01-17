using System.Security.Cryptography;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;

namespace CVA.Infrastructure.Storage.S3;

/// <summary>
/// Minimal S3-compatible object storage wrapper.
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
    /// Downloads an object as bytes.
    /// </summary>
    /// <param name="key">Object key.</param>
    /// <param name="ct">Cancellation token.</param>
    public async Task<byte[]> GetBytesAsync(string key, CancellationToken ct)
    {
        var request = new GetObjectRequest
        {
            BucketName = options.Value.Bucket,
            Key = key
        };

        using var response = await s3.GetObjectAsync(request, ct);
        await using var rs = response.ResponseStream;

        await using var ms = new MemoryStream();
        await rs.CopyToAsync(ms, ct);
        return ms.ToArray();
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
    /// Computes SHA-256 hex string.
    /// </summary>
    /// <param name="data">Input bytes.</param>
    public static string Sha256Hex(byte[] data)
    {
        var hash = SHA256.HashData(data);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}