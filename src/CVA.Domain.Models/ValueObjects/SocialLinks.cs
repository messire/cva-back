namespace CVA.Domain.Models;

/// <summary>
/// Represents the developer's social media links.
/// </summary>
/// <param name="LinkedIn">The developer's LinkedIn profile URL.</param>
/// <param name="GitHub">The developer's GitHub profile URL.</param>
/// <param name="Twitter">The developer's Twitter profile URL.</param>
/// <param name="Telegram">The developer's Telegram profile URL.</param>
public sealed record SocialLinks(Url? LinkedIn, Url? GitHub, Url? Twitter, Url? Telegram)
{
    /// <summary>
    /// Creates a new instance of <see cref="SocialLinks"/>.
    /// </summary>
    /// <param name="linkedIn">The developer's LinkedIn profile URL.</param>
    /// <param name="github">The developer's GitHub profile URL.</param>
    /// <param name="twitter">The developer's Twitter profile URL.</param>
    /// <param name="telegram">The developer's Telegram profile URL.</param>
    /// <returns>The created social links.</returns>
    public static SocialLinks Create(string? linkedIn, string? github, string? twitter, string? telegram)
        => new(Url.TryFrom(linkedIn), Url.TryFrom(github), Url.TryFrom(twitter), Url.TryFrom(telegram));
}