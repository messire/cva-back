namespace CVA.Application.Contracts;

/// <summary>
/// Authentication token pair DTO.
/// </summary>
/// <param name="AccessToken">JWT access token.</param>
/// <param name="RefreshToken">Opaque refresh token (one-time use, rotated on refresh).</param>
/// <param name="UserId">User identifier (sub).</param>
public sealed record AuthTokenDto(string AccessToken, string RefreshToken, Guid UserId);