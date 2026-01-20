using CVA.Domain.Models;

namespace CVA.Tests.Unit.Domain.ValueObjects;

/// <summary>
/// Unit tests for the <see cref="SocialLinks"/> value object.
/// </summary>
[Trait(Layer.Domain, Category.ValueObjects)]
public class SocialLinksTests
{
    /// <summary>
    /// Purpose: Verify that Create method correctly initializes social links.
    /// Should: Set all properties correctly from provided arguments.
    /// When: Valid URL strings are provided.
    /// </summary>
    [Theory]
    [InlineData("https://linkedin.com/in/user", "https://github.com/user", "https://twitter.com/user", "https://t.me/user")]
    public void Create_Should_Initialize_SocialLinks(string linkedIn, string github, string twitter, string telegram)
    {
        // Act
        var socialLinks = SocialLinks.Create(linkedIn, github, twitter, telegram);

        // Assert
        Assert.Equal(linkedIn, socialLinks.LinkedIn?.Value);
        Assert.Equal(github, socialLinks.GitHub?.Value);
        Assert.Equal(twitter, socialLinks.Twitter?.Value);
        Assert.Equal(telegram, socialLinks.Telegram?.Value);
    }

    /// <summary>
    /// Purpose: Verify that Create method handles null values correctly.
    /// Should: Set properties to null.
    /// When: All arguments are null.
    /// </summary>
    [Fact]
    public void Create_Should_Handle_Null_Values()
    {
        // Act
        var socialLinks = SocialLinks.Create(null, null, null, null);

        // Assert
        Assert.Null(socialLinks.LinkedIn);
        Assert.Null(socialLinks.GitHub);
        Assert.Null(socialLinks.Twitter);
        Assert.Null(socialLinks.Telegram);
    }

    /// <summary>
    /// Purpose: Ensure that invalid URLs are rejected in Create method.
    /// Should: Throw ArgumentException.
    /// When: An invalid URL string is provided for any of the social links.
    /// </summary>
    [Theory]
    [InlineData("invalid-url", null, null, null)]
    public void Create_Should_Throw_When_Url_Is_Invalid(string linkedIn, string? github, string? twitter, string? telegram)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => SocialLinks.Create(linkedIn, github, twitter, telegram));
    }
}
