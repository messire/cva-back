namespace CVA.Infrastructure.Common;

/// <summary>
/// Specifies the types of databases that can be used.
/// </summary>
public enum DatabaseType
{
    /// <summary>
    /// Represents the absence of a specified database type.
    /// </summary>
    None = 0,

    /// <summary>
    /// Represents the MongoDB database type.
    /// </summary>
    Mongo,

    /// <summary>
    /// Represents the PostgreSQL database type.
    /// </summary>
    Postgres,
}