using CVA.Application.Abstractions.Media;

namespace CVA.Application.ProfileService.UpdateProfileAvatar;

/// <summary>
/// Handles uploading and setting a new avatar for the current user.
/// Replaces the previous avatar (if it was stored in our media storage).
/// </summary>
/// <param name="repository">The developer profile repository.</param>
/// <param name="userAccessor">The current user accessor.</param>
/// <param name="mediaStorage">Media storage used to persist avatar images.</param>
public sealed class UpdateProfileAvatarHandler(IDeveloperProfileRepository repository, ICurrentUserAccessor userAccessor, IMediaStorage mediaStorage)
    : ICommandHandler<UpdateProfileAvatarCommand, ProfileDto>
{
    /// <inheritdoc />
    public async Task<Result<ProfileDto>> HandleAsync(UpdateProfileAvatarCommand command, CancellationToken ct)
    {
        var profile = await repository.GetByIdAsync(userAccessor.UserId, ct);
        if (profile is null)
        {
            return Result<ProfileDto>.Fail("Profile not found.");
        }

        var now = DateTimeOffset.UtcNow;
        var oldRelativePath = TryGetRelativeMediaPath(profile.Avatar?.ImageUrl.Value, command.MediaRequestPath);
        if (!string.IsNullOrWhiteSpace(oldRelativePath))
        {
            await mediaStorage.DeleteAsync(oldRelativePath, ct);
        }

        var newRelativePath = await mediaStorage.SaveAvatarAsync(userAccessor.UserId, command.Content, command.ContentType, ct);
        var newAvatarUrl = BuildAbsoluteMediaUrl(command.PublicBaseUrl, command.MediaRequestPath, newRelativePath);
        profile.ChangeAvatar(Avatar.From(newAvatarUrl), now);
        var updatedProfile = await repository.UpdateAsync(profile, ct);
        return updatedProfile?.ToDto() ?? Result<ProfileDto>.Fail("Failed to update profile.");
    }

    private static string BuildAbsoluteMediaUrl(string publicBaseUrl, string mediaRequestPath, string relativePath)
    {
        var baseUrl = publicBaseUrl.TrimEnd('/');
        var mediaPath = NormalizeMediaPath(mediaRequestPath);
        var rel = relativePath.TrimStart('/');
        return $"{baseUrl}{mediaPath}/{rel}";
    }

    private static string NormalizeMediaPath(string mediaRequestPath)
    {
        var path = mediaRequestPath.Trim();
        if (!path.StartsWith('/'))
        {
            path = "/" + path;
        }

        return path.TrimEnd('/');
    }

    private static string? TryGetRelativeMediaPath(string? absoluteUrl, string mediaRequestPath)
    {
        if (string.IsNullOrWhiteSpace(absoluteUrl)) return null;
        if (!Uri.TryCreate(absoluteUrl, UriKind.Absolute, out var uri)) return null;

        var mediaPath = NormalizeMediaPath(mediaRequestPath);
        return uri.AbsolutePath.StartsWith(mediaPath + "/", StringComparison.OrdinalIgnoreCase)
            ? uri.AbsolutePath[(mediaPath.Length + 1)..].TrimStart('/')
            : null;
    }
}