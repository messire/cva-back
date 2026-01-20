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

/// <summary>
/// Represents a collection of tests that require a full application factory with Postgres and Mongo fixtures.
/// </summary>
[CollectionDefinition(nameof(ApiCollection))]
public class ApiCollection : ICollectionFixture<PostgresFixture>, ICollectionFixture<MongoFixture>;

/// <summary>
/// Web application collection.
/// </summary>
[CollectionDefinition(nameof(WebCollection))]
public class WebCollection : ICollectionFixture<PostgresFixture>, ICollectionFixture<MongoFixture>;