using CVA.Application.Contracts;

namespace CVA.Presentation.Auth;

/// <summary>
/// Stores short-lived one-time authentication exchange codes.
/// </summary>
public interface IOneTimeCodeStore
{
    /// <summary>
    /// Creates a one-time exchange code.
    /// </summary>
    /// <param name="token">Authentication token pair.</param>
    /// <param name="ttl">Time-to-live.</param>
    /// <returns>Generated one-time code.</returns>
    string Create(AuthTokenDto token, TimeSpan ttl);

    /// <summary>
    /// Consumes a one-time exchange code.
    /// </summary>
    /// <param name="code">One-time code.</param>
    /// <param name="token">Authentication token pair.</param>
    /// <returns>True if code was valid and consumed.</returns>
    bool TryConsume(string code, out AuthTokenDto token);
}