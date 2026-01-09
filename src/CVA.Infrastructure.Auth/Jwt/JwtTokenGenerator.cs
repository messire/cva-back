using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CVA.Infrastructure.Auth;

/// <summary>
/// Generates JWT access tokens for authenticated users.
/// </summary>
/// <param name="options">JWT configuration options.</param>
internal sealed class JwtTokenGenerator(IOptions<JwtOptions> options) : IAppTokenIssuer
{
    private readonly JwtOptions _options = options.Value ?? throw new ArgumentNullException(nameof(options));

    /// <inheritdoc/>
    public string Issue(Guid userId, string role)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId cannot be empty.", nameof(userId));
        }

        if (string.IsNullOrWhiteSpace(role))
        {
            throw new ArgumentException("Role cannot be empty.", nameof(role));
        }

        if (string.IsNullOrWhiteSpace(_options.SigningKey))
        {
            throw new InvalidOperationException("Jwt:SigningKey is not configured.");
        }

        var now = DateTime.UtcNow;
        var expires = now.AddMinutes(_options.LifetimeMinutes <= 0 ? 60 : _options.LifetimeMinutes);

        var claims = new[]
        {
            new Claim(CustomClaimTypes.Subject, userId.ToString()),
            new Claim(CustomClaimTypes.Role, role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            notBefore: now,
            expires: expires,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}