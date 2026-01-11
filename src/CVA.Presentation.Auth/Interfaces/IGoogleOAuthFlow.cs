using Microsoft.AspNetCore.Mvc;

namespace CVA.Presentation.Auth;

/// <summary>
/// Google-specific OAuth redirect flow abstraction.
/// </summary>
public interface IGoogleOAuthFlow
{
    /// <summary>
    /// Starts Google OAuth redirect flow.
    /// </summary>
    /// <param name="returnUrl">
    /// Absolute URL to which the user should be redirected after authentication.
    /// </param>
    /// <returns>HTTP redirect result.</returns>
    IActionResult Start(string? returnUrl);

    /// <summary>
    /// Handles Google OAuth callback.
    /// </summary>
    /// <param name="code">Google authorization code.</param>
    /// <param name="state">OAuth state.</param>
    /// <param name="error">OAuth error code.</param>
    /// <param name="errorDescription">OAuth error description.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>HTTP redirect result.</returns>
    Task<IActionResult> Callback(string? code, string? state, string? error, string? errorDescription, CancellationToken ct);
}