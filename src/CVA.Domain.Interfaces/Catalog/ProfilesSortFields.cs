namespace CVA.Domain.Interfaces;

/// <summary>
/// Supported sorting fields for profiles catalog.
/// </summary>
public static class ProfilesSortFields
{
    /// <summary>
    /// Sort by last update timestamp.
    /// </summary>
    public const string UpdatedAt = "updatedAt";

    /// <summary>
    /// Sort by person's name
    /// (LastName, FirstName).
    /// </summary>
    public const string Name = "name";

    /// <summary>
    /// Sort by profile identifier.
    /// </summary>
    public const string Id = "id";
}