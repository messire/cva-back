using System.Text;
using System.Text.Json;
using CVA.Application.Contracts;
using CVA.Infrastructure.Storage.S3;
using Microsoft.Extensions.Options;

namespace CVA.Infrastructure.ResumePdf;

/// <summary>
/// Orchestrates resume PDF generation + caching in S3-compatible storage.
/// Cache key is content-based (SHA-256 of profile DTO), so no DB schema changes needed.
/// </summary>
/// <param name="options">Resume PDF options.</param>
/// <param name="storage">S3 storage.</param>
/// <param name="renderer">Playwright renderer.</param>
public sealed class ResumePdfService(IOptions<ResumePdfOptions> options, S3ObjectStorage storage, PlaywrightResumePdfRenderer renderer)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    /// <summary>
    /// Gets (or generates) a resume PDF for the provided profile and returns a presigned download URL.
    /// </summary>
    /// <param name="profile">Developer profile DTO.</param>
    /// <param name="ct">Cancellation token.</param>
    public async Task<(byte[] Content, string FileName)> GetOrCreateAsync(DeveloperProfileDto profile, CancellationToken ct)
    {
        var hash = ComputeProfileHash(profile);
        var key = $"{options.Value.CachePrefix.TrimEnd('/')}/{profile.Id:D}/{hash}.pdf";

        if (!await storage.ExistsAsync(key, ct))
        {
            var url = BuildProfileUrl(profile.Id);
            var pdf = await renderer.RenderAsync(url, ct);

            await using var ms = new MemoryStream(pdf);
            await storage.PutAsync(key, ms, "application/pdf", ct);
        }

        var bytes = await storage.GetBytesAsync(key, ct);
        var fileName = BuildFileName(profile);

        return (bytes, fileName);
    }

    private string BuildProfileUrl(Guid profileId)
    {
        var baseUrl = options.Value.FrontendBaseUrl.TrimEnd('/');
        var path = options.Value.ProfilePathTemplate.Replace("{id}", profileId.ToString("D"));
        if (!path.StartsWith('/'))
        {
            path = "/" + path;
        }

        return baseUrl + path;
    }

    private static string ComputeProfileHash(DeveloperProfileDto profile)
    {
        var json = JsonSerializer.Serialize(profile, JsonOptions);
        var bytes = Encoding.UTF8.GetBytes(json);
        return S3ObjectStorage.Sha256Hex(bytes);
    }

    private static string BuildFileName(DeveloperProfileDto profile)
    {
        var first = (profile.FirstName ?? string.Empty).Trim();
        var last = (profile.LastName ?? string.Empty).Trim();
        var name = $"{first}_{last}".Trim('_');

        return string.IsNullOrWhiteSpace(name)
            ? "resume.pdf"
            : $"{name}_resume.pdf";
    }
}