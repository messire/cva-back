namespace CVA.Domain.Models;

/// <summary>
/// Represents a project name.
/// </summary>
/// <param name="Value">The name of the project.</param>
public sealed record ProjectName(string Value)
{
    /// <summary>
    /// Creates a new <see cref="ProjectName"/> instance from the specified value.
    /// </summary>
    /// <param name="value">The project name value.</param>
    /// <returns>The created project name.</returns>
    public static ProjectName From(string value)
        => new (Ensure.TrimToNull(value, nameof(value)));
}