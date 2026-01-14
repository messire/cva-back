using CVA.Application.ProfileService.UpdateProfileAvatar;
using CVA.Application.ProfileService.UpdateProjectImage;
using CVA.Infrastructure.Common.Media;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace CVA.Presentation.Web;

/// <summary>
/// Owner-only API for uploading media for "my" developer profile (avatar and project images).
/// </summary>
/// <param name="commands">Command executor.</param>
/// <param name="mediaOptions">Media storage options.</param>
[ApiController]
[Authorize]
[Route("api/profile")]
public sealed class ProfileMediaController(
    CommandExecutor commands,
    IOptions<MediaOptions> mediaOptions) : ControllerBase
{
    private const long AvatarMaxBytes = 500 * 1024;
    private const long ProjectImageMaxBytes = 500 * 1024;

    private static readonly string[] AllowedImageContentTypes =
    [
        "image/jpeg",
        "image/png",
        "image/webp"
    ];

    /// <summary>
    /// Uploads and sets the current user's avatar image.
    /// Replaces the old avatar (if present).
    /// </summary>
    /// <param name="file">Image file (multipart/form-data, field name: "file").</param>
    /// <param name="ct">Cancellation token.</param>
    [HttpPost("avatar")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(AvatarMaxBytes)]
    [RequestFormLimits(MultipartBodyLengthLimit = AvatarMaxBytes)]
    [ProducesResponseType(typeof(DeveloperProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UploadAvatar([FromForm] IFormFile file, CancellationToken ct)
    {
        if (file is null || file.Length <= 0)
        {
            return Problem(detail: "File is required.", statusCode: StatusCodes.Status400BadRequest, title: "Validation");
        }

        if (file.Length > AvatarMaxBytes)
        {
            return Problem(
                detail: $"Avatar is too large. Max size is {AvatarMaxBytes} bytes.",
                statusCode: StatusCodes.Status400BadRequest,
                title: "Validation");
        }

        var contentType = NormalizeContentType(file.ContentType);

        if (string.IsNullOrWhiteSpace(contentType) || contentType.Equals("application/octet-stream", StringComparison.OrdinalIgnoreCase))
        {
            return Problem(
                detail: "Invalid file type. Please upload an image file (JPEG/PNG/WebP), not a link.",
                statusCode: StatusCodes.Status400BadRequest,
                title: "Validation");
        }

        if (!AllowedImageContentTypes.Contains(contentType, StringComparer.OrdinalIgnoreCase))
        {
            return Problem(
                detail: "Unsupported file type. Allowed: image/jpeg, image/png, image/webp.",
                statusCode: StatusCodes.Status400BadRequest,
                title: "Validation");
        }

        var publicBaseUrl = BuildPublicBaseUrl();
        var mediaRequestPath = mediaOptions.Value.PublicRequestPath;

        await using var stream = file.OpenReadStream();

        var command = new UpdateProfileAvatarCommand(
            Content: stream,
            ContentLength: file.Length,
            ContentType: contentType,
            PublicBaseUrl: publicBaseUrl,
            MediaRequestPath: mediaRequestPath);

        var result = await commands.ExecuteAsync<UpdateProfileAvatarCommand, DeveloperProfileDto>(command, ct);
        return this.ToActionResult(result);
    }

    /// <summary>
    /// Uploads and sets the image for an existing project.
    /// Replaces the old project image (if present).
    /// </summary>
    /// <param name="projectId">Project id.</param>
    /// <param name="file">Image file (multipart/form-data, field name: "file").</param>
    /// <param name="ct">Cancellation token.</param>
    [HttpPost("projects/{projectId:guid}/image")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ProjectImageMaxBytes)]
    [RequestFormLimits(MultipartBodyLengthLimit = ProjectImageMaxBytes)]
    [ProducesResponseType(typeof(DeveloperProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UploadProjectImage(Guid projectId, [FromForm] IFormFile file, CancellationToken ct)
    {
        if (file is null || file.Length <= 0)
        {
            return Problem(detail: "File is required.", statusCode: StatusCodes.Status400BadRequest, title: "Validation");
        }

        if (file.Length > ProjectImageMaxBytes)
        {
            return Problem(
                detail: $"Project image is too large. Max size is {ProjectImageMaxBytes} bytes.",
                statusCode: StatusCodes.Status400BadRequest,
                title: "Validation");
        }

        var contentType = NormalizeContentType(file.ContentType);

        if (string.IsNullOrWhiteSpace(contentType) || contentType.Equals("application/octet-stream", StringComparison.OrdinalIgnoreCase))
        {
            return Problem(
                detail: "Invalid file type. Please upload an image file (JPEG/PNG/WebP), not a link.",
                statusCode: StatusCodes.Status400BadRequest,
                title: "Validation");
        }

        if (!AllowedImageContentTypes.Contains(contentType, StringComparer.OrdinalIgnoreCase))
        {
            return Problem(
                detail: "Unsupported file type. Allowed: image/jpeg, image/png, image/webp.",
                statusCode: StatusCodes.Status400BadRequest,
                title: "Validation");
        }

        var publicBaseUrl = BuildPublicBaseUrl();
        var mediaRequestPath = mediaOptions.Value.PublicRequestPath;

        await using var stream = file.OpenReadStream();

        var command = new UpdateProjectImageCommand(
            ProjectId: projectId,
            Content: stream,
            ContentLength: file.Length,
            ContentType: contentType,
            PublicBaseUrl: publicBaseUrl,
            MediaRequestPath: mediaRequestPath);

        var result = await commands.ExecuteAsync<UpdateProjectImageCommand, DeveloperProfileDto>(command, ct);
        return this.ToActionResult(result);
    }

    private static string NormalizeContentType(string? contentType)
        => (contentType ?? string.Empty).Trim();

    private string BuildPublicBaseUrl()
        => $"{Request.Scheme}://{Request.Host}";
}