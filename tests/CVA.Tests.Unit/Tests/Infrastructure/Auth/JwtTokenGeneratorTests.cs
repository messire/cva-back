using System.IdentityModel.Tokens.Jwt;
using CVA.Infrastructure.Auth;
using Microsoft.Extensions.Options;

namespace CVA.Tests.Unit.Infrastructure.Auth;

/// <summary>
/// Unit tests for <see cref="JwtTokenGenerator"/>.
/// </summary>
[Trait(Layer.Infrastructure, Category.Services)]
public class JwtTokenGeneratorTests
{
    /// <summary>
    /// Purpose: Verify JWT contains expected claims and expiration.
    /// Should: Include sub, role, issuer, audience, and exp within configured window.
    /// When: Issuing a token for a valid user and role.
    /// </summary>
    [Fact]
    public void Issue_Should_Create_Token_With_Sub_Role_And_Exp()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var role = "Admin";
        var options = Options.Create(new JwtOptions
        {
            Issuer = "cva-tests",
            Audience = "cva-tests",
            SigningKey = "SuperSecretTestKeyThatIsLongEnough123!",
            LifetimeMinutes = 30
        });
        var generator = new JwtTokenGenerator(options);
        var now = DateTime.UtcNow;

        // Act
        var tokenString = generator.Issue(userId, role);
        var token = new JwtSecurityTokenHandler().ReadJwtToken(tokenString);

        // Assert
        Assert.Equal(options.Value.Issuer, token.Issuer);
        Assert.Contains(options.Value.Audience, token.Audiences);
        Assert.Equal(userId.ToString(), token.Claims.Single(c => c.Type == CustomClaimTypes.Subject).Value);
        Assert.Equal(role, token.Claims.Single(c => c.Type == CustomClaimTypes.Role).Value);
        Assert.InRange(token.ValidTo, now.AddMinutes(29), now.AddMinutes(31));
    }

    /// <summary>
    /// Purpose: Verify invalid inputs are rejected.
    /// Should: Throw for empty user id or role.
    /// When: Issuing tokens with invalid arguments.
    /// </summary>
    [Fact]
    public void Issue_Should_Throw_On_Invalid_Arguments()
    {
        // Arrange
        var options = Options.Create(new JwtOptions
        {
            Issuer = "cva-tests",
            Audience = "cva-tests",
            SigningKey = "SuperSecretTestKeyThatIsLongEnough123!"
        });
        var generator = new JwtTokenGenerator(options);

        // Act + Assert
        Assert.Throws<ArgumentException>(() => generator.Issue(Guid.Empty, "User"));
        Assert.Throws<ArgumentException>(() => generator.Issue(Guid.NewGuid(), " "));
    }
}
