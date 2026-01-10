namespace CVA.Domain.Models;

/// <summary>
/// Represents a technology tag.
/// </summary>
/// <param name="Value">The technology tag value.</param>
public sealed record TechTag(string Value)
{
    /// <summary>
    /// Creates a new <see cref="TechTag"/> instance from the specified value.
    /// </summary>
    /// <param name="value">The technology tag value.</param>
    /// <returns>The created technology tag.</returns>
    public static TechTag From(string value)
        => new (Ensure.TrimToNull(value, nameof(value)));
}