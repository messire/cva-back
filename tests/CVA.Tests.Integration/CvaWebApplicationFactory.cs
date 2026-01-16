using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace CVA.Tests.Integration;

public class CvaWebApplicationFactory(PostgresFixture postgresFixture, MongoFixture mongoFixture)
    : WebApplicationFactory<Program>
{
    /// <inheritdoc />
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            var uri = new Uri(postgresFixture.ConnectionString);
            var testConfig = new Dictionary<string, string?>
            {
                ["Database:Type"] = "Postgres",
                ["Database:Postgres:Connection:Host"] = uri.Host,
                ["Database:Postgres:Connection:Port"] = uri.Port.ToString(), 
                ["Database:Postgres:Connection:Database"] = uri.AbsolutePath.TrimStart('/'),
                ["Database:Postgres:Connection:User"] = "postgres",
                ["Database:Postgres:Connection:Password"] = "postgres",
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