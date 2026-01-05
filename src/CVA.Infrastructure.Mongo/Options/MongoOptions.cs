namespace CVA.Infrastructure.Mongo;

/// <summary>
/// Configuration options for Mongo database integration.
/// </summary>
internal sealed class MongoOptions
{
    /// <summary>
    /// Represents the configuration key used to retrieve Mongo options from the configuration system.
    /// </summary>
    public const string Path = "Database:Mongo";

    /// <summary>
    /// Represents the connection string used to establish a connection to the Mongo database.
    /// </summary>
    public required string Connection { get; set; }

    /// <summary>
    /// Specifies the name of the Mongo database to be used in the application's database configuration.
    /// </summary>
    public required string DatabaseName { get; set; }
}