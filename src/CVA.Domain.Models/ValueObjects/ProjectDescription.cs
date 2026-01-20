namespace CVA.Domain.Models;

/// <summary>
/// Represents a project description.
/// </summary>
/// <param name="Value">The description of the project.</param>
public sealed record ProjectDescription(string Value)
{
    /// <summary>
    /// Creates a new <see cref="ProjectDescription"/> instance from the specified value.
    /// </summary>
    /// <param name="value">The project description value.</param>
    /// <returns>The created project description.</returns>
    public static ProjectDescription From(string value)
        => new (Ensure.TrimToNull(value, nameof(value)));

    /// <summary>
    /// Tries to create a new <see cref="ProjectDescription"/> instance from the specified value.
    /// </summary>
    /// <param name="value">The project description value.</param>
    /// <returns>The created project description or null if the value is null or empty.</returns>
    public static ProjectDescription? TryFrom(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : From(value!);
}