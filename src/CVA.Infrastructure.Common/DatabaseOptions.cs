namespace CVA.Infrastructure.Common;

/// <summary>
/// Represents configuration options for a database.
/// </summary>
public class DatabaseOptions
{
    /// <summary>
    /// The static default path representing the database configuration section in the settings.
    /// Used to locate database-specific configuration options within a configuration file.
    /// </summary>
    public static string Path = "Database";

    /// <summary>
    /// Specifies the type of database associated with the configuration.
    /// </summary>
    public DatabaseType Type { get; set; }
}