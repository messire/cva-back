using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace CVA.Tests.Integration;

/// <summary>
/// Test host factory configured with Postgres and Mongo fixtures.
/// </summary>
public class CvaWebApplicationFactory(PostgresFixture postgresFixture, MongoFixture mongoFixture)
    : WebApplicationFactory<Program>
{
    /// <inheritdoc />
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
        {
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder(postgresFixture.ConnectionString);
            var testConfig = new Dictionary<string, string?>
            {
                ["Database:Type"] = "Postgres",
                ["Database:Postgres:Connection:Host"] = connectionStringBuilder.Host,
                ["Database:Postgres:Connection:Port"] = connectionStringBuilder.Port.ToString(),
                ["Database:Postgres:Connection:Database"] = connectionStringBuilder.Database,
                ["Database:Postgres:Connection:User"] = connectionStringBuilder.Username,
                ["Database:Postgres:Connection:Password"] = connectionStringBuilder.Password,
                ["Database:Postgres:Pooling:Lifetime"] = "0",
                ["Database:Postgres:Pooling:MaxSize"] = "100",
                ["Database:Postgres:Pooling:MinSize"] = "0",
                ["Database:Postgres:Security:SslMode"] = "Disable",
                ["Database:Postgres:Security:TrustServerCertificate"] = "true",
                ["Jwt:SigningKey"] = IntegrationTestAuthHelper.SigningKey,
                ["Jwt:Issuer"] = IntegrationTestAuthHelper.Issuer,
                ["Jwt:Audience"] = IntegrationTestAuthHelper.Audience
            };

            config.AddInMemoryCollection(testConfig);
        });
    }
}