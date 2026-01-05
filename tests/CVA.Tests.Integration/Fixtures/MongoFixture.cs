using CVA.Tests.Integration.Network;

namespace CVA.Tests.Integration.Fixtures;

/// <summary>
/// Postgres fixture.
/// </summary>
public class MongoFixture : IAsyncLifetime
{
    private readonly INetwork _network;
    private readonly MongoDbContainer _container;

    /// <summary>
    /// Initializes a new instance of the <see cref="MongoFixture"/> class.
    /// </summary>
    public MongoFixture()
    {
        _network = Initializer.Init();
        _container = (MongoDbContainer)Mongo.Initializer.Init(_network);
    }

    /// <summary>
    /// MongoDb connection string.
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