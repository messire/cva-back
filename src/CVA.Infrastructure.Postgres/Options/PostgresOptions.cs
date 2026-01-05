using Npgsql;

namespace CVA.Infrastructure.Postgres;

/// <summary>
/// Configuration options for Postgres database integration.
/// </summary>
internal sealed class PostgresOptions
{
    /// <summary>
    /// Represents the configuration key used to retrieve Postgres options from the configuration system.
    /// </summary>
    public const string Path = "Database:Postgres";

    /// <summary>
    /// Represents the connection string used to establish a connection to the Postgres database.
    /// </summary>
    public required ConnectionsOptions Connection { get; set; }

    /// <summary>
    /// Pooling settings
    /// </summary>
    public required PoolingOptions Pooling { get; set; }

    /// <summary>
    /// Security settings
    /// </summary>
    public required SecurityOptions Security { get; set; }

    /// <summary>
    /// Connection settings
    /// </summary>
    public class ConnectionsOptions
    {
        /// <summary>
        /// Specifies the Postgres database host address.
        /// </summary>
        public required string Host { get; set; }

        /// <summary>
        /// Specifies the Postgres database port.
        /// </summary>
        public required int Port { get; set; }

        /// <summary>
        /// Specifies the Postgres database name.
        /// </summary>
        public required string Database { get; set; }

        /// <summary>
        /// Specifies the Postgres database user.
        /// </summary>
        public required string User { get; set; }

        /// <summary>
        /// Specifies the Postgres database password.
        /// </summary>
        public required string Password { get; set; }
    }

    /// <summary>
    /// Pooling settings
    /// </summary>
    public class PoolingOptions
    {
        /// <summary>
        /// Connection lifetime.
        /// </summary>
        public required int Lifetime { get; set; }

        /// <summary>
        /// Max pool connection size.
        /// </summary>
        public required int MaxSize { get; set; }

        /// <summary>
        /// Min pool connection size.
        /// </summary>
        public required int MinSize { get; set; }
    }

    public class SecurityOptions
    {
        /// <summary>
        /// SSL Mode for PostgreSQL connection.
        /// </summary>
        public required SslMode SslMode { get; set; }

        /// <summary>
        /// Trust server certificate
        /// </summary>
        public required bool TrustServerCertificate { get; set; }

        /// <summary>
        /// Path to root certificate file.
        /// </summary>
        public string? RootCertificate { get; set; }
    }
}