using CVA.Tests.Integration.Network;

namespace CVA.Tests.Integration.Fixtures;

/// <summary>
/// Postgres fixture.
/// </summary>
public class PostgresFixture : IAsyncLifetime
{
    private readonly INetwork _network;
    private readonly PostgreSqlContainer _container;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostgresFixture"/> class.
    /// </summary>
    public PostgresFixture()
    {
        _network = Initializer.Init();
        _container = (PostgreSqlContainer)Postgres.Initializer.Init(_network);
    }

    /// <summary>
    /// Postgres connection string.
    /// </summary>
    public string ConnectionString => _container.GetConnectionString();

    /// <inheritdoc />
    public async Task InitializeAsync()
    {
        await _network.CreateAsync();
        await _container.StartAsync();
    }

    /// <inheritdoc />
    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
        await _network.DisposeAsync();
    }
}