namespace CVA.Domain.Models;

/// <summary>
/// Represents a profile summary.
/// </summary>
/// <param name="Value">The profile summary.</param>
public sealed record ProfileSummary(string Value)
{
    /// <summary>
    /// Creates a new <see cref="ProfileSummary"/> instance from the specified value.
    /// </summary>
    /// <param name="value">The profile summary value.</param>
    /// <returns>The created profile summary.</returns>
    public static ProfileSummary From(string value)
        => new (Ensure.TrimToNull(value, nameof(value)));

    /// <summary>
    /// Tries to create a new <see cref="ProfileSummary"/> instance from the specified value.
    /// </summary>
    /// <param name="value">The profile summary value.</param>
    /// <returns>The created profile summary or null if the value is null or empty.</returns>
    public static ProfileSummary? TryFrom(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : From(value!);
}