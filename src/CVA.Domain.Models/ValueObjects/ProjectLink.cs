namespace CVA.Domain.Models;

/// <summary>
/// Represents a project link.
/// </summary>
/// <param name="Value">The URL of the project link.</param>
public sealed record ProjectLink(Url? Value)
{
    /// <summary>
    /// Creates a new <see cref="ProjectLink"/> instance from the specified URL.
    /// </summary>
    /// <param name="url">The URL of the project link.</param>
    /// <returns>A new <see cref="ProjectLink"/> instance.</returns>
    public static ProjectLink From(string? url)
        => new (Url.TryFrom(url));
}