using CVA.Presentation.Auth;

namespace CVA.Tests.Unit.Presentation.Auth;

/// <summary>
/// Unit tests for <see cref="GoogleAuthorizeUrlBuilder"/>.
/// </summary>
[Trait(Layer.Application, Category.Services)]
public class GoogleAuthorizeUrlBuilderTests
{
    /// <summary>
    /// Purpose: Verify the authorize URL contains required query parameters.
    /// Should: Include response_type, client_id, redirect_uri, scope, and state.
    /// When: Building a Google OAuth authorize URL.
    /// </summary>
    [Fact]
    public void Build_Should_Include_Required_Parameters()
    {
        // Arrange
        const string clientId = "client-id";
        const string redirectUri = "https://example.com/callback";
        const string state = "state-123";
        var builder = new GoogleAuthorizeUrlBuilder(clientId);

        // Act
        var url = builder.Build(redirectUri, state);

        // Assert
        Assert.StartsWith("https://accounts.google.com/o/oauth2/v2/auth?", url);
        Assert.Contains("response_type=code", url);
        Assert.Contains("client_id=" + Uri.EscapeDataString(clientId), url);
        Assert.Contains("redirect_uri=" + Uri.EscapeDataString(redirectUri), url);
        Assert.Contains("scope=" + Uri.EscapeDataString("openid email profile"), url);
        Assert.Contains("state=" + Uri.EscapeDataString(state), url);
    }
}
