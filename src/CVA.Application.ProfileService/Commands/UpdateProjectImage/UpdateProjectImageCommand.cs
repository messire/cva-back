namespace CVA.Application.ProfileService.UpdateProjectImage;

/// <summary>
/// Command to upload and set a new image for an existing project.
/// The new image replaces the old one.
/// </summary>
/// <param name="ProjectId">The identifier of the project.</param>
/// <param name="Content">Project image content stream.</param>
/// <param name="ContentLength">Project image size in bytes.</param>
/// <param name="ContentType">Project image MIME type.</param>
/// <param name="PublicBaseUrl">Absolute base URL of the API, used to build an absolute media URL.</param>
/// <param name="MediaRequestPath">Public request path for media files.</param>
public sealed record UpdateProjectImageCommand(Guid ProjectId, Stream Content, long ContentLength, string ContentType, string PublicBaseUrl, string MediaRequestPath)
    : ICommand<ProfileDto>;