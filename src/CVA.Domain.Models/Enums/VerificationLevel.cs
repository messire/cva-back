namespace CVA.Domain.Models;

/// <summary>
/// Verification level.
/// </summary>
public enum VerificationLevel
{
    /// <summary>
    /// Not verified.
    /// </summary>
    NotVerified = 0,

    /// <summary>
    /// Fake.
    /// </summary>
    Fake,

    /// <summary>
    /// Verified.
    /// </summary>
    Verified,

    /// <summary>
    /// Premium.
    /// </summary>
    Premium
}