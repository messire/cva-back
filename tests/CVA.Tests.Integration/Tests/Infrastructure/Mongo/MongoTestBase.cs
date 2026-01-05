using CVA.Domain.Interfaces;
using CVA.Infrastructure.Mongo;
using CVA.Tests.Integration.Fixtures;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace CVA.Tests.Integration.Tests.Infrastructure.Mongo;

/// <summary>
/// Provides a base class for integration tests that require interaction with MongoDB.
/// </summary>
[Collection(nameof(MongoCollection))]
public abstract class MongoTestBase : IAsyncLifetime
{
    /// <summary>
    /// Represents the database fixture used for setting up MongoDB integration testing environments.
    /// </summary>
    protected readonly MongoFixture Fixture;

    /// <summary>
    /// Represents the client to interact with a MongoDB instance, providing methods for working with databases.
    /// </summary>
    protected readonly IMongoClient MongoClient;

    /// <summary>
    /// Represents the configuration options for integrating with a MongoDB database.
    /// </summary>
    internal readonly MongoOptions MongoOptions;

    /// <summary>
    /// Represents a cancellation token source.
    /// </summary>
    protected readonly CancellationTokenSource Cts;

    /// <summary>
    /// Initializes static members of the <see cref="MongoTestBase"/> class.
    /// </summary>
    static MongoTestBase()
    {
        try
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        }
        catch (BsonSerializationException)
        {
            //skip
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MongoTestBase"/> class with the provided database fixture.
    /// </summary>
    /// <param name="fixture">The database fixture used for setting up MongoDB integration testing environments.</param>
    protected internal MongoTestBase(MongoFixture fixture)
    {
        Fixture = fixture;
        MongoOptions = Integration.Mongo.Tools.GetConfiguration(Fixture.ConnectionString);
        MongoClient = new MongoClient(Fixture.ConnectionString);
        Cts = new CancellationTokenSource();
    }

    /// <summary>
    /// Creates and initializes a new instance of the <see cref="UserMongoRepository"/> class.
    /// </summary>
    /// <returns></returns>
    protected IUserRepository CreateRepository()
        => new UserMongoRepository(MongoClient, MongoOptions);

    /// <inheritdoc />
    public virtual async Task InitializeAsync()
    {
        var database = MongoClient.GetDatabase(MongoOptions.DatabaseName);
        var collections = await database.ListCollectionNamesAsync();
        while (await collections.MoveNextAsync())
        {
            foreach (var name in collections.Current)
            {
                await database.DropCollectionAsync(name);
            }
        }
    }

    /// <inheritdoc />
    public async Task DisposeAsync()
    {
        await Cts.CancelAsync();
    }
}