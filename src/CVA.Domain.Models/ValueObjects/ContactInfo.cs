namespace CVA.Domain.Models;

/// <summary>
/// Represents the developer's contact information.
/// </summary>
/// <param name="Location">The developer's location.</param>
/// <param name="Email">The developer's email address.</param>
/// <param name="Website">The developer's website URL.</param>
public sealed record ContactInfo(Location? Location, EmailAddress Email, Url? Website)
{
    /// <summary>
    /// Creates a new instance of <see cref="ContactInfo"/>.
    /// </summary>
    /// <param name="location">The developer's location.</param>
    /// <param name="email">The developer's email address.</param>
    /// <param name="website">The developer's website URL.</param>
    /// <returns>The created contact information.</returns>
    public static ContactInfo Create(Location? location, EmailAddress email, Url? website)
    {
        Ensure.NotNull(email, nameof(email));
        return new ContactInfo(location, email, website);
    }
}