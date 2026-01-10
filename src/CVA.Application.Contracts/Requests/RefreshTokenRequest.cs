namespace CVA.Application.Contracts;

/// <summary>
/// Request model for refreshing an access token.
/// </summary>
/// <param name="RefreshToken">Refresh token issued by the API.</param>
public sealed record RefreshTokenRequest(string RefreshToken = "");