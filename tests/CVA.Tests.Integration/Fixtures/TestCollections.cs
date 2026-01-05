namespace CVA.Tests.Integration.Fixtures;

/// <summary>
/// Represents a collection of tests that require a MongoDb database fixture for setup and execution.
/// </summary>
[CollectionDefinition(nameof(MongoCollection))]
public class MongoCollection : ICollectionFixture<MongoFixture>;

/// <summary>
/// Represents a collection of tests that require a PostgreSQL database fixture for setup and execution.
/// </summary>
[CollectionDefinition(nameof(PostgresCollection))]
public class PostgresCollection : ICollectionFixture<PostgresFixture>;