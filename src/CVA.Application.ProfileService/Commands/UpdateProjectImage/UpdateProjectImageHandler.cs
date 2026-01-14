using CVA.Application.Abstractions.Media;

namespace CVA.Application.ProfileService.UpdateProjectImage;

/// <summary>
/// Handles the upload and update of a project image in the developer profile.
/// Replaces the previous project image (if it was stored in our media storage).
/// </summary>
/// <param name="repository">The developer profile repository.</param>
/// <param name="userAccessor">The current user accessor.</param>
/// <param name="mediaStorage">Media storage used to persist project images.</param>
public sealed class UpdateProjectImageHandler(IDeveloperProfileRepository repository, ICurrentUserAccessor userAccessor, IMediaStorage mediaStorage)
    : ICommandHandler<UpdateProjectImageCommand, DeveloperProfileDto>
{
    /// <inheritdoc />
    public async Task<Result<DeveloperProfileDto>> HandleAsync(UpdateProjectImageCommand command, CancellationToken ct)
    {
        var profile = await repository.GetByIdAsync(userAccessor.UserId, ct);
        if (profile is null)
        {
            return Result<DeveloperProfileDto>.Fail("Profile not found.");
        }

        var now = DateTimeOffset.UtcNow;
        var projectId = new ProjectId(command.ProjectId);
        var project = profile.Projects.FirstOrDefault(item => item.Id.Equals(projectId));
        if (project is null)
        {
            return Result<DeveloperProfileDto>.Fail("Project not found.");
        }

        var oldRelativePath = TryGetRelativeMediaPath(project.Icon?.ImageUrl.Value, command.MediaRequestPath);
        if (!string.IsNullOrWhiteSpace(oldRelativePath))
        {
            await mediaStorage.DeleteAsync(oldRelativePath!, ct);
        }

        var newRelativePath = await mediaStorage.SaveProjectImageAsync(userAccessor.UserId, projectId.Value, command.Content, command.ContentType, ct);
        var newImageUrl = BuildAbsoluteMediaUrl(command.PublicBaseUrl, command.MediaRequestPath, newRelativePath);
        var newIcon = ProjectIcon.From(newImageUrl);
        profile.UpdateProject(projectId, project.Name, project.Description, newIcon, project.Link, project.TechStack, now);
        var updatedProfile = await repository.UpdateAsync(profile, ct);
        return updatedProfile?.ToDto() ?? Result<DeveloperProfileDto>.Fail("Failed to update profile.");
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