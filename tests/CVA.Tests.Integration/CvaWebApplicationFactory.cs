using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace CVA.Tests.Integration;

public class CvaWebApplicationFactory(PostgresFixture postgresFixture, MongoFixture mongoFixture)
    : WebApplicationFactory<Program>
{
    /// <inheritdoc />
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            var builder = new NpgsqlConnectionStringBuilder(postgresFixture.ConnectionString);
            var testConfig = new Dictionary<string, string?>
            {
                ["Database:Type"] = "Postgres",
                ["Database:Postgres:Connection:Host"] = builder.Host,
                ["Database:Postgres:Connection:Port"] = builder.Port.ToString(),
                ["Database:Postgres:Connection:Database"] = builder.Database,
                ["Database:Postgres:Connection:User"] = builder.Username,
                ["Database:Postgres:Connection:Password"] = builder.Password,
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