using CVA.Application.Contracts;

namespace CVA.Tests.Common;

/// <summary>
/// A custom specimen builder for creating <see cref="SocialLinksDto"/> instances.
/// Ensures that generated values are valid absolute URLs.
/// </summary>
public sealed class SocialLinksDtoBuilder : ISpecimenBuilder
{
    /// <summary>
    /// Gets the singleton instance of the <see cref="SocialLinksDtoBuilder"/> class.
    /// </summary>
    public static SocialLinksDtoBuilder Instance => new();

    /// <inheritdoc />
    public object Create(object request, ISpecimenContext context)
    {
        if (request is not Type type || type != typeof(SocialLinksDto)) return new NoSpecimen();

        return new SocialLinksDto
        {
            LinkedIn = CreateUrl(context),
            GitHub = CreateUrl(context),
            Twitter = CreateUrl(context),
            Telegram = CreateUrl(context)
        };
    }

    private static string CreateUrl(ISpecimenContext context)
        => $"https://{context.Create<Guid>():N}.test";
}
