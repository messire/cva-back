namespace CVA.Domain.Models;

/// <summary>
/// Represents a role title.
/// </summary>
/// <param name="Value">The role title.</param>
public sealed record RoleTitle(string Value)
{
    /// <summary>
    /// Creates a new <see cref="RoleTitle"/> instance from the specified value.
    /// </summary>
    /// <param name="value">The role title value.</param>
    /// <returns>The created role title.</returns>
    public static RoleTitle From(string value)
        => new RoleTitle(Ensure.TrimToNull(value, nameof(value)));

    /// <summary>
    /// Tries to create a new <see cref="RoleTitle"/> instance from the specified value.
    /// </summary>
    /// <param name="value">The role title value.</param>
    /// <returns>The created role title or null if the value is null or empty.</returns>
    public static RoleTitle? TryFrom(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : From(value!);
}