using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;

namespace CVA.Infrastructure.Auth;

/// <summary>
/// Default implementation of <see cref="IRefreshTokenProtector"/>.
/// Generates cryptographically-strong tokens and hashes them with a server-side pepper.
/// </summary>
internal sealed class RefreshTokenProtector(IOptions<RefreshTokenOptions> options) : IRefreshTokenProtector
{
    private readonly RefreshTokenOptions _options = options.Value
                                                    ?? throw new ArgumentNullException(nameof(options));

    /// <inheritdoc />
    public string Generate()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        return Convert.ToBase64String(bytes);
    }

    /// <inheritdoc />
    public string Hash(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            throw new ArgumentException("Refresh token is required.", nameof(refreshToken));
        }

        if (string.IsNullOrWhiteSpace(_options.Pepper))
        {
            throw new InvalidOperationException("RefreshTokens:Pepper is not configured.");
        }

        var input = $"{_options.Pepper}|{refreshToken}";
        var stableHash = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(stableHash);
    }
}