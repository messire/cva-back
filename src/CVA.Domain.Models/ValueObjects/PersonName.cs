namespace CVA.Domain.Models;

/// <summary>
/// Represents a person's name.
/// </summary>
/// <param name="FirstName">The person's first name.</param>
/// <param name="LastName">The person's last name.</param>
public sealed record PersonName(string FirstName, string LastName)
{
    /// <summary>
    /// Creates a new instance of <see cref="PersonName"/>.
    /// </summary>
    /// <param name="firstName">The person's first name.</param>
    /// <param name="lastName">The person's last name.</param>
    /// <returns>The created person name.</returns>
    public static PersonName From(string firstName, string lastName)
    {
        firstName = Ensure.TrimToNull(firstName, nameof(firstName));
        lastName = Ensure.TrimToNull(lastName, nameof(lastName));
        return new PersonName(firstName, lastName);
    }
}