namespace CVA.Application.Contracts;

/// <summary>
/// Request for exchanging a one-time authorization code for application JWT tokens.
/// </summary>
public sealed class AuthCodeExchangeRequest
{
    /// <summary>
    /// One-time code issued by the backend after successful OAuth callback.
    /// </summary>
    public string Code { get; set; } = string.Empty;
}