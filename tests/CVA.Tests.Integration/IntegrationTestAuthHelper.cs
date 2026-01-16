using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CVA.Infrastructure.Auth;
using Microsoft.IdentityModel.Tokens;

namespace CVA.Tests.Integration;

/// <summary>
/// Helper for generating JWTs used in integration tests.
/// </summary>
public static class IntegrationTestAuthHelper
{
    /// <summary>
    /// The signing key used for generating and validating JWT tokens.
    /// </summary>
    public const string SigningKey = "SuperSecretTestKeyThatIsLongEnough123!";

    /// <summary>
    /// The issuer claim value for JWT tokens.
    /// </summary>
    public const string Issuer = "CVA.Tests";

    /// <summary>
    /// The audience claim value for JWT tokens.
    /// </summary>
    public const string Audience = "CVA.Tests";

    /// <summary>
    /// Generates a signed JWT for a specific user and role.
    /// </summary>
    /// <param name="userId">User identifier to include in claims.</param>
    /// <param name="role">Role claim value.</param>
    /// <returns>Serialized JWT string.</returns>
    public static string GenerateJwt(Guid userId, string role = "User")
    {
        var claims = new[]
        {
            new Claim(CustomClaimTypes.Subject, userId.ToString()),
            new Claim(CustomClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}