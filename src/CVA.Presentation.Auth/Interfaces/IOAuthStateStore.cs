namespace CVA.Presentation.Auth;

/// <summary>
/// Stores OAuth state tokens used for CSRF protection.
/// </summary>
public interface IOAuthStateStore
{
    /// <summary>
    /// Creates a new OAuth state entry.
    /// </summary>
    /// <param name="returnUrl">Frontend return URL.</param>
    /// <param name="ttl">Time-to-live.</param>
    /// <returns>Generated state value.</returns>
    string Create(string returnUrl, TimeSpan ttl);

    /// <summary>
    /// Consumes an OAuth state entry.
    /// </summary>
    /// <param name="state">State value.</param>
    /// <param name="returnUrl">Associated return URL.</param>
    /// <returns>True if state was valid and consumed.</returns>
    bool TryConsume(string state, out string returnUrl);
}