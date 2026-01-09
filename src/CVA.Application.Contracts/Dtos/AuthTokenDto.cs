namespace CVA.Application.Contracts;

/// <summary>
/// Authentication token DTO.
/// </summary>
/// <param name="AccessToken">JWT access token.</param>
/// <param name="UserId">User identifier (sub).</param>
public sealed record AuthTokenDto(string AccessToken, Guid UserId);