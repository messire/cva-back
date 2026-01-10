namespace CVA.Domain.Models;

/// <summary>
/// Represents a work description.
/// </summary>
/// <param name="Value">The work description.</param>
public sealed record WorkDescription(string Value)
{
    /// <summary>
    /// The maximum length of a work description.
    /// </summary>
    public const int MaxLength = 1000;

    /// <summary>
    /// Creates a new <see cref="WorkDescription"/> instance from the specified value.
    /// </summary>
    /// <param name="value">The work description value.</param>
    /// <returns>The created work description.</returns>
    public static WorkDescription From(string value)
    {
        value = Ensure.TrimToNull(value, nameof(value));
        if (value.Length <= MaxLength) return new WorkDescription(value);
        throw new ArgumentOutOfRangeException(nameof(value), $"Max {MaxLength} chars");
    }

    /// <summary>
    /// Tries to create a new <see cref="WorkDescription"/> instance from the specified value.
    /// </summary>
    /// <param name="value">The work description value.</param>
    /// <returns>The created work description or null if the value is null or empty.</returns>
    public static WorkDescription? TryFrom(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : From(value!);
}