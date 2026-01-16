using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;

namespace CVA.Tests.Integration;

[Collection(nameof(PostgresCollection))]
public abstract class IntegrationTestBase : IAsyncLifetime
{
    private readonly CvaWebApplicationFactory _factory;
    protected readonly HttpClient Client;
    protected readonly IServiceScope Scope;

    protected IntegrationTestBase(PostgresFixture postgresFixture)
    {
        _factory = new CvaWebApplicationFactory(postgresFixture, new MongoFixture());
        Client = _factory.CreateClient();
        Scope = _factory.Services.CreateScope();
    }

    protected void Authenticate(Guid userId, string role = "User")
    {
        var token = IntegrationTestAuthHelper.GenerateJwt(userId, role);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public virtual Task InitializeAsync() => Task.CompletedTask;

    public virtual async Task DisposeAsync()
    {
        Scope.Dispose();
        Client.Dispose();
        await _factory.DisposeAsync();
    }
}
