namespace CVA.Domain.Interfaces;

/// <summary>
/// Represents a repository interface for managing refresh tokens.
/// </summary>
public interface IRefreshTokenRepository
{
    /// <summary>
    /// Creates a new refresh token.
    /// </summary>
    /// <param name="refreshToken">Refresh token to create.</param>
    /// <param name="ct">Cancellation token.</param>
    Task CreateAsync(RefreshToken refreshToken, CancellationToken ct);

    /// <summary>
    /// Gets a refresh token by its hashed value.
    /// </summary>
    /// <param name="tokenHash">Token hash.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<RefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken ct);

    /// <summary>
    /// Marks a refresh token as revoked.
    /// </summary>
    /// <param name="id">Refresh token identifier.</param>
    /// <param name="revokedAt">Revocation time (UTC).</param>
    /// <param name="replacedByTokenHash">Replacement token hash, if rotated.</param>
    /// <param name="ct">Cancellation token.</param>
    Task RevokeAsync(Guid id, DateTimeOffset revokedAt, string? replacedByTokenHash, CancellationToken ct);
}