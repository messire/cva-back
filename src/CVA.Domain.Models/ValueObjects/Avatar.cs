namespace CVA.Domain.Models;

/// <summary>
/// Represents an avatar image.
/// </summary>
/// <param name="ImageUrl">The URL of the avatar image.</param>
public sealed record Avatar(Url ImageUrl)
{
    /// <summary>
    /// Creates a new <see cref="Avatar"/> instance from the specified URL.
    /// </summary>
    /// <param name="url">The URL of the avatar image.</param>
    /// <returns>The created avatar.</returns>
    public static Avatar From(string url) => new Avatar(Url.From(url));

    /// <summary>
    /// Tries to create a new <see cref="Avatar"/> instance from the specified URL.
    /// </summary>
    /// <param name="url">The URL of the avatar image.</param>
    /// <returns>The created avatar or null if the URL is null or empty.</returns>
    public static Avatar? TryFrom(string? url)
        => string.IsNullOrWhiteSpace(url) ? null : From(url!);
}