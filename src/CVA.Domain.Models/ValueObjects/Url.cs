namespace CVA.Domain.Models;

/// <summary>
/// Represents a URL.
/// </summary>
/// <param name="Value">The URL value.</param>
public sealed record Url(string Value)
{
    /// <summary>
    /// Creates a new <see cref="Url"/> instance from the specified value.
    /// </summary>
    /// <param name="value">The URL value.</param>
    /// <returns>The created URL.</returns>
    /// <exception cref="ArgumentException">Thrown if the provided value is not a valid URL.</exception>
    public static Url From(string value)
    {
        value = Ensure.TrimToNull(value, nameof(value));
        return Uri.TryCreate(value, UriKind.Absolute, out _)
            ? new Url(value)
            : throw new ArgumentException("Invalid url.", nameof(value));
    }

    /// <summary>
    /// Tries to create a new <see cref="Url"/> instance from the specified value.
    /// </summary>
    /// <param name="value">The URL value.</param>
    /// <returns>The created URL or null if the value is invalid or null.</returns>
    public static Url? TryFrom(string? value)
        => !string.IsNullOrWhiteSpace(value)
            ? From(value)
            : null;
}