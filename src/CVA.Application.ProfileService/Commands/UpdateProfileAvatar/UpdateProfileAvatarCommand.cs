namespace CVA.Application.ProfileService.UpdateProfileAvatar;

/// <summary>
/// Command to upload and set a new avatar for the current user.
/// The new avatar replaces the old one.
/// </summary>
/// <param name="Content">Avatar image content stream.</param>
/// <param name="ContentLength">Avatar image size in bytes.</param>
/// <param name="ContentType">Avatar image MIME type.</param>
/// <param name="PublicBaseUrl">Absolute base URL of the API, used to build an absolute media URL.</param>
/// <param name="MediaRequestPath">Public request path for media files.</param>
public sealed record UpdateProfileAvatarCommand(Stream Content, long ContentLength, string ContentType, string PublicBaseUrl, string MediaRequestPath)
    : ICommand<DeveloperProfileDto>;