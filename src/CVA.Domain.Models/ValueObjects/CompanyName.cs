namespace CVA.Domain.Models;

/// <summary>
/// Represents a company name.
/// </summary>
/// <param name="Value">The name of the company.</param>
public sealed record CompanyName(string Value)
{
    /// <summary>
    /// Creates a new <see cref="CompanyName"/> instance from the specified value.
    /// </summary>
    /// <param name="value">The company name value.</param>
    /// <returns>The created company name.</returns>
    public static CompanyName From(string value)
        => new (Ensure.TrimToNull(value, nameof(value)));
}