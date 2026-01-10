namespace CVA.Domain.Models;

/// <summary>
/// Represents the developer's email address.
/// </summary>
/// <param name="Value">The email address.</param>
public sealed record EmailAddress(string Value)
{
    /// <summary>
    /// Creates a new <see cref="EmailAddress"/> instance from the specified value.
    /// </summary>
    /// <param name="value">The email address value.</param>
    /// <returns>The created email address.</returns>
    /// <exception cref="ArgumentException">Thrown if the provided value is not a valid email address.</exception>
    public static EmailAddress From(string value)
    {
        value = Ensure.TrimToNull(value, nameof(value));
        return value.Contains('@')
            ? new EmailAddress(value)
            : throw new ArgumentException("Invalid email.", nameof(value));
    }
}