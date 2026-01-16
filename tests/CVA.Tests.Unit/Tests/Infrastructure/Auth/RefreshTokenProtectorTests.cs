using CVA.Infrastructure.Auth;
using Microsoft.Extensions.Options;

namespace CVA.Tests.Unit.Infrastructure.Auth;

/// <summary>
/// Unit tests for <see cref="RefreshTokenProtector"/>.
/// </summary>
[Trait(Layer.Infrastructure, Category.Services)]
public class RefreshTokenProtectorTests
{
    /// <summary>
    /// Purpose: Verify refresh tokens are generated and can be hashed.
    /// Should: Generate non-empty tokens and stable hashes.
    /// When: Using a configured pepper value.
    /// </summary>
    [Fact]
    public void Generate_And_Hash_Should_Work_With_Configured_Pepper()
    {
        // Arrange
        var options = Options.Create(new RefreshTokenOptions
        {
            Pepper = "pepper-value"
        });
        var protector = new RefreshTokenProtector(options);

        // Act
        var token = protector.Generate();
        var hash1 = protector.Hash(token);
        var hash2 = protector.Hash(token);

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(token));
        Assert.Equal(hash1, hash2);
    }

    /// <summary>
    /// Purpose: Verify invalid inputs are rejected.
    /// Should: Throw when refresh token is missing or pepper is not configured.
    /// When: Hashing invalid refresh tokens.
    /// </summary>
    [Fact]
    public void Hash_Should_Throw_On_Invalid_Input()
    {
        // Arrange
        var validOptions = Options.Create(new RefreshTokenOptions { Pepper = "pepper-value" });
        var withPepper = new RefreshTokenProtector(validOptions);
        var missingPepper = new RefreshTokenProtector(Options.Create(new RefreshTokenOptions { Pepper = "" }));

        // Act + Assert
        Assert.Throws<ArgumentException>(() => withPepper.Hash(" "));
        Assert.Throws<InvalidOperationException>(() => missingPepper.Hash("token"));
    }
}
