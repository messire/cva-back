namespace CVA.Application.Abstractions.Media;

/// <summary>
/// Abstraction for persistent storage of user-uploaded media files.
/// </summary>
public interface IMediaStorage
{
    /// <summary>
    /// Saves a user avatar image.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="content">Image content stream.</param>
    /// <param name="contentType">MIME type of the image.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Relative storage path of the saved image.</returns>
    Task<string> SaveAvatarAsync(Guid userId, Stream content, string contentType, CancellationToken ct);

    /// <summary>
    /// Saves an image associated with a project.
    /// </summary>
    /// <param name="userId">Owner user identifier.</param>
    /// <param name="projectId">Project identifier.</param>
    /// <param name="content">Image content stream.</param>
    /// <param name="contentType">MIME type of the image.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Relative storage path of the saved image.</returns>
    Task<string> SaveProjectImageAsync(Guid userId, Guid projectId, Stream content, string contentType, CancellationToken ct);

    /// <summary>
    /// Deletes a previously stored media file.
    /// </summary>
    /// <param name="relativePath">Relative storage path.</param>
    /// <param name="ct">Cancellation token.</param>
    Task DeleteAsync(string relativePath, CancellationToken ct);
}