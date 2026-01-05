namespace CVA.Domain.Models;

/// <summary>
/// Represents a normalized email address.
/// </summary>
public readonly record struct EmailObject
{
    /// <summary>
    /// The normalized email address value.
    /// </summary>

    public string Value { get; }

    private EmailObject(string value)
        => Value = value;

    /// <summary>
    /// Creates a new instance of the <see cref="EmailObject"/> struct with a validated and normalized email address.
    /// </summary>
    /// <param name="value">The email address as a string.</param>
    /// <returns>A new <see cref="EmailObject"/> instance containing the normalized email address.</returns>
    /// <exception cref="DomainValidationException">Thrown when the provided email address is null, empty, whitespace, or invalid.</exception>
    public static EmailObject Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainValidationException("Email must not be empty.");
        }

        var normalized = value.Trim().ToLowerInvariant();

        var at = normalized.IndexOf('@');
        if (at <= 0 || at == normalized.Length - 1)
        {
            throw new DomainValidationException("Email is invalid.");
        }

        return new EmailObject(normalized);
    }

    /// <inheritdoc />
    public override string ToString() => Value;

    /// <summary>
    /// Defines an implicit conversion from an <see cref="EmailObject"/> instance to its string representation.
    /// </summary>
    /// <param name="email">The <see cref="EmailObject"/> instance to convert.</param>
    /// <returns>The string representation of the email address.</returns>
    public static implicit operator string(EmailObject email) => email.Value;
}