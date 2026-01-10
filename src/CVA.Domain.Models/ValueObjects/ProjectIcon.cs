namespace CVA.Domain.Models;

/// <summary>
/// Represents a project icon.
/// </summary>
/// <param name="ImageUrl">The URL of the project icon.</param>
public sealed record ProjectIcon(Url ImageUrl)
{
    /// <summary>
    /// Creates a new <see cref="ProjectIcon"/> instance from the specified URL.
    /// </summary>
    /// <param name="url">The URL of the project icon.</param>
    /// <returns>The created project icon.</returns>
    public static ProjectIcon From(string url)
        => new (Url.From(url));

    /// <summary>
    /// Tries to create a new <see cref="ProjectIcon"/> instance from the specified URL.
    /// </summary>
    /// <param name="url">The URL of the project icon.</param>
    /// <returns>The created project icon or null if the URL is null or empty.</returns>
    public static ProjectIcon? TryFrom(string? url)
        => string.IsNullOrWhiteSpace(url) ? null : From(url!);
}