namespace CVA.Domain.Models;

public sealed partial class User
{
    /// <summary>
    /// Creates a new user aggregate.
    /// </summary>
    public static User Create(string name, string surname, string email)
    {
        var user = new User();
        user.ChangeName(name, surname);
        user.ChangeEmail(email);
        return user;
    }

    /// <summary>
    /// Reconstructs a user aggregate from persistence with the provided attributes.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <param name="name">The first name of the user.</param>
    /// <param name="surname">The last name of the user.</param>
    /// <param name="email">The email address of the user.</param>
    /// <param name="phone">The phone number of the user, if available.</param>
    /// <param name="photo">Optional URL of the user's profile picture.</param>
    /// <param name="birthday">The date of birth of the user, if available.</param>
    /// <param name="summaryInfo">The summary or additional information about the user, if available.</param>
    /// <param name="skills">A collection of skills associated with the user, if available.</param>
    /// <param name="workExperience">A collection of work experiences associated with the user, if available.</param>
    /// <returns>A reconstructed user aggregate instance.</returns>
    /// <exception cref="DomainValidationException">Thrown when <paramref name="id"/> is an empty GUID.</exception>
    public static User FromPersistence(
        Guid id,
        string name,
        string surname,
        string email,
        string? phone,
        string? photo,
        DateOnly? birthday,
        string? summaryInfo,
        IEnumerable<string>? skills,
        IEnumerable<Work>? workExperience)
    {
        if (id == Guid.Empty)
            throw new DomainValidationException("Id must not be empty.");

        var user = new User(id, name, surname, email);
        user.UpdateProfile(phone, birthday, summaryInfo);
        user.UpdatePhoto(photo);
        user.ReplaceSkills(skills);
        user.ReplaceWorkExperience(workExperience);

        return user;
    }

    /// <summary>
    /// Changes user's name.
    /// </summary>
    public void ChangeName(string name, string surname)
    {
        Name = RequireNonEmpty(name, nameof(name));
        Surname = RequireNonEmpty(surname, nameof(surname));
    }

    /// <summary>
    /// Changes user's email.
    /// </summary>
    public void ChangeEmail(string email)
        => Email = EmailObject.Create(email);

    /// <summary>
    /// Updates optional profile fields.
    /// </summary>
    public void UpdateProfile(string? phone, DateOnly? birthday, string? summaryInfo)
    {
        Phone = phone?.Trim();
        Birthday = birthday;
        SummaryInfo = summaryInfo?.Trim();
    }

    /// <summary>
    /// Updates the user's photo information.
    /// </summary>
    /// <param name="photo">The new photo to associate with the user. Null or whitespace will result in clearing the current photo.</param>
    public void UpdatePhoto(string? photo)
        => Photo = photo?.Trim();

    /// <summary>
    /// Replaces current skills list with a new set.
    /// </summary>
    public void ReplaceSkills(IEnumerable<string>? skills)
        => ReplaceList(_skills, skills, value => value.Trim());

    /// <summary>
    /// Replaces user's work experience list with a new collection.
    /// </summary>
    public void ReplaceWorkExperience(IEnumerable<Work>? works)
        => ReplaceList(_workExperience, works);

    /// <summary>
    /// Adds a new work entry.
    /// </summary>
    public void AddWork(Work work)
    {
        ArgumentNullException.ThrowIfNull(work);
        _workExperience.Add(work);
    }

    /// <summary>
    /// Removes work entries matching the predicate.
    /// </summary>
    public int RemoveWork(Func<Work, bool> predicate)
        => predicate is not null
            ? _workExperience.RemoveAll(work => predicate(work))
            : throw new ArgumentNullException(nameof(predicate));

    private static string RequireNonEmpty(string value, string paramName)
        => !string.IsNullOrWhiteSpace(value)
            ? value.Trim()
            : throw new DomainValidationException($"Value for '{paramName}' must not be empty.");
}