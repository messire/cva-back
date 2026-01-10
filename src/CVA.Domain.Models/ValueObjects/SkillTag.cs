namespace CVA.Domain.Models;

/// <summary>
/// Represents a skill tag.
/// </summary>
/// <param name="Value">The skill tag value.</param>
public sealed record SkillTag(string Value)
{
    /// <summary>
    /// Creates a new <see cref="SkillTag"/> instance from the specified value.
    /// </summary>
    /// <param name="value">The skill tag value.</param>
    /// <returns>The created skill tag.</returns>
    public static SkillTag From(string value)
        => new (Ensure.TrimToNull(value, nameof(value)));
}