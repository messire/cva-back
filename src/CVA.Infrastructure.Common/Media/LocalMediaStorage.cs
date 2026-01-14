using CVA.Application.Abstractions.Media;
using Microsoft.Extensions.Options;

namespace CVA.Infrastructure.Common.Media;

/// <summary>
/// Local file system implementation of <see cref="IMediaStorage"/>.
/// Intended for usage with persistent volumes.
/// </summary>
public sealed class LocalMediaStorage(IOptions<MediaOptions> options) : IMediaStorage
{
    /// <inheritdoc />
    public async Task<string> SaveAvatarAsync(Guid userId, Stream content, string contentType, CancellationToken ct)
    {
        var extension = ResolveExtension(contentType);
        var relativePath = $"avatars/{userId:D}/avatar_{Guid.NewGuid():N}{extension}";
        await SaveAsync(relativePath, content, ct);
        return relativePath;
    }

    /// <inheritdoc />
    public async Task<string> SaveProjectImageAsync(Guid userId, Guid projectId, Stream content, string contentType, CancellationToken ct)
    {
        var extension = ResolveExtension(contentType);
        var relativePath = $"projects/{userId:D}/{projectId:D}/image_{Guid.NewGuid():N}{extension}";
        await SaveAsync(relativePath, content, ct);
        return relativePath;
    }

    /// <inheritdoc />
    public Task DeleteAsync(string relativePath, CancellationToken ct)
    {
        var fullPath = ToFullPath(relativePath);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        return Task.CompletedTask;
    }

    private async Task SaveAsync(string relativePath, Stream content, CancellationToken ct)
    {
        var fullPath = ToFullPath(relativePath);
        var directory = Path.GetDirectoryName(fullPath);

        if (directory is not null)
        {
            Directory.CreateDirectory(directory);
        }

        await using var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None);
        await content.CopyToAsync(fileStream, ct);
    }

    private string ToFullPath(string relativePath)
    {
        var normalized = relativePath.Replace('/', Path.DirectorySeparatorChar);
        return Path.Combine(options.Value.RootPath, normalized);
    }

    private static string ResolveExtension(string contentType) =>
        contentType.ToLowerInvariant() switch
        {
            "image/jpeg" => ".jpg",
            "image/png" => ".png",
            "image/webp" => ".webp",
            _ => throw new InvalidOperationException($"Unsupported image content type: {contentType}")
        };
}