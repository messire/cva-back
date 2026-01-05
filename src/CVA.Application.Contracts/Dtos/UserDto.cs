namespace CVA.Application.Contracts;

/// <summary>
/// Represents a data transfer object for user information.
/// </summary>
/// <param name="Name">The name of the user.</param>
/// <param name="Surname">The surname of the user.</param>
/// <param name="Email">The email address of the user.</param>
public record UserDto(string Name, string Surname, string Email)
{
    /// <summary>
    /// The unique identifier for the user.
    /// </summary>
    public Guid? Id { get; init; }

    /// <summary>
    /// The phone number associated with the user.
    /// </summary>
    public string? Phone { get; init; }

    /// <summary>
    /// The URL of the user's profile picture.
    /// </summary>
    public string? Photo { get; init; }

    /// <summary>
    /// The birthdate of the user.
    /// </summary>
    public DateOnly? Birthday { get; init; }

    /// <summary>
    /// A brief summary or description of the user's profile or background.
    /// </summary>
    public string? SummaryInfo { get; init; }

    /// <summary>
    /// A collection of skills associated with the user.
    /// </summary>
    public string[]? Skills { get; init; }

    /// <summary>
    /// A collection of work experiences associated with the user.
    /// </summary>
    public WorkDto[]? WorkExperience { get; init; }
}