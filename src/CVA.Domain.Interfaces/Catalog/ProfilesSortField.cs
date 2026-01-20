namespace CVA.Domain.Interfaces;

/// <summary>
/// Available sort fields for profiles catalog.
/// </summary>
public enum ProfilesSortField
{
    /// <summary>
    /// Undefined sort field.
    /// </summary>
    Undefined = 0,

    /// <summary>
    /// Sort by last update timestamp.
    /// </summary>
    UpdatedAt,

    /// <summary>
    /// Sort by person's name'
    /// </summary>
    Name,

    /// <summary>
    /// Sort by profile identifier.
    /// </summary>
    Id
}