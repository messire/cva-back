namespace CVA.Domain.Models;

/// <summary>
/// Represents the developer's phone number.
/// </summary>
/// <param name="Value">The phone number.</param>
public sealed record PhoneNumber(string Value)
{
    /// <summary>
    /// Creates a new <see cref="PhoneNumber"/> instance from the specified value.
    /// </summary>
    /// <param name="value">The phone number value.</param>
    /// <returns>The created phone number.</returns>
    public static PhoneNumber From(string value)
    {
        value = Ensure.TrimToNull(value, nameof(value));
        return new PhoneNumber(value);
    }

    /// <summary>
    /// Creates a new <see cref="PhoneNumber"/> instance from the specified value, or returns null if the value is null or empty.
    /// </summary>
    /// <param name="value">The phone number value.</param>
    /// <returns>The created phone number or null.</returns>
    public static PhoneNumber? TryFrom(string? value)
        => value is not null ? new PhoneNumber(value) : null;
}
