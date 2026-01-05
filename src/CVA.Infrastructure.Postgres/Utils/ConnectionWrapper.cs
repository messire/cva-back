using Npgsql;

namespace CVA.Infrastructure.Postgres;

/// <summary>
/// Utility class to wrap a PostgreSQL connection string with custom database options.
/// </summary>
internal static class ConnectionWrapper
{
    /// <summary>
    /// Creates a PostgreSQL connection string based on the provided <see cref="PostgresOptions"/>.
    /// </summary>
    /// <param name="options">Instance of <see cref="PostgresOptions"/>.</param>
    /// <returns>A configured connection string.</returns>
    public static string Wrap(PostgresOptions options)
    {
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = options.Connection.Host,
            Port = options.Connection.Port,
            Database = options.Connection.Database,
            Username = options.Connection.User,
            Password = options.Connection.Password,
            Pooling = true,
            MinPoolSize = options.Pooling.MinSize,
            MaxPoolSize = options.Pooling.MaxSize,
            ConnectionLifetime = options.Pooling.Lifetime,
            SslMode = options.Security.SslMode,
#pragma warning disable CS0618 // Type or member is obsolete
            TrustServerCertificate = options.Security.TrustServerCertificate,
#pragma warning restore CS0618 // Type or member is obsolete
            RootCertificate = options.Security.RootCertificate
        };

        return builder.ToString();
    }
}