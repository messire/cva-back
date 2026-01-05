using CVA.Infrastructure.Postgres;
using Npgsql;

namespace CVA.Tests.Integration.Postgres;

internal static class Tools
{
    public static PostgresOptions GetConfiguration(string connectionString)
        => GetPostgresOptions(connectionString);

    private static PostgresOptions GetPostgresOptions(string connectionString)
    {
        var builder = new NpgsqlConnectionStringBuilder(connectionString);
        return new()
        {
            Connection = new()
            {
                Host = builder.Host ?? string.Empty,
                Port = builder.Port,
                Database = builder.Database ?? string.Empty,
                User = builder.Username ?? string.Empty,
                Password = builder.Password ?? string.Empty
            },
            Pooling = new()
            {
                Lifetime = 300,
                MaxSize = 10,
                MinSize = 1
            },
            Security = new()
            {
                SslMode = builder.SslMode,
#pragma warning disable CS0618 // Type or member is obsolete
                TrustServerCertificate = builder.TrustServerCertificate,
#pragma warning restore CS0618 // Type or member is obsolete
                RootCertificate = builder.RootCertificate
            }
        };
    }
}