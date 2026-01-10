using Microsoft.AspNetCore.Mvc;

namespace CVA.Presentation.Auth;

/// <summary>
/// Orchestrates Google OAuth redirect-based authentication flow.
/// </summary>
internal sealed class GoogleOAuthFlow : IGoogleOAuthFlow
{
    /// <inheritdoc />
    public IActionResult Start(string? returnUrl)
        => throw new NotImplementedException();

    /// <inheritdoc />
    public Task<IActionResult> Callback(string? code, string? state, string? error, string? errorDescription, CancellationToken ct)
        => throw new NotImplementedException();
}