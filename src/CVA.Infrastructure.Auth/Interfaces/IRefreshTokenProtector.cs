namespace CVA.Infrastructure.Auth;

/// <summary>
/// Generates and validates refresh tokens.
/// </summary>
public interface IRefreshTokenProtector
{
    /// <summary>
    /// Generates a new refresh token value (opaque string).
    /// </summary>
    string Generate();

    /// <summary>
    /// Computes a stable hash for persisting the refresh token.
    /// </summary>
    /// <param name="refreshToken">Opaque refresh token.</param>
    string Hash(string refreshToken);
}