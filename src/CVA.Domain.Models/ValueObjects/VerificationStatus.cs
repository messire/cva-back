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
}