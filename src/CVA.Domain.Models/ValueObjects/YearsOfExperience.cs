namespace CVA.Domain.Models;

/// <summary>
/// Represents the developer's years of experience.
/// </summary>
/// <param name="Value">The number of years of experience.</param>
public sealed record YearsOfExperience(int Value)
{
    /// <summary>
    /// Creates a new <see cref="YearsOfExperience"/> instance from the specified value.
    /// </summary>
    /// <param name="value">The number of years of experience.</param>
    /// <returns>The created years of experience.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the value is out of range (0-80).</exception>
    public static YearsOfExperience From(int value)
        => value is >= 0 and <= 80
            ? new YearsOfExperience(value)
            : throw new ArgumentOutOfRangeException(nameof(value));
}