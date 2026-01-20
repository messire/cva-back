namespace CVA.Domain.Models;

/// <summary>
/// Represents the developer's verification status.
/// </summary>
/// <param name="Value">The verification status.</param>
public sealed record VerificationStatus(VerificationLevel Value)
{
    /// <summary>
    /// Returns true if the developer is verified.
    /// </summary>
    public bool IsVerified =>
        Value is VerificationLevel.Verified or VerificationLevel.Premium;

    /// <summary>
    /// Default verification status.
    /// </summary>
    public static VerificationStatus Default =>
        new(VerificationLevel.NotVerified);

    /// <summary>
    /// Tries to create a new <see cref="VerificationStatus"/> instance from the specified string value.
    /// </summary>
    /// <param name="value">The verification status value as a string.</param>
    /// <returns>The created verification status or the default status if the value is invalid.</returns>
    public static VerificationStatus TryFrom(string? value)
        => Enum.TryParse<VerificationLevel>(value, true, out var level)
            ? new VerificationStatus(level)
            : Default;
}