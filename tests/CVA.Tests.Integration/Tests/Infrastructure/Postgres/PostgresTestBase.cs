using CVA.Domain.Models;
using CVA.Infrastructure.Postgres;
using CVA.Tests.Integration.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace CVA.Tests.Integration.Tests.Infrastructure.Postgres;

/// <summary>
/// Base class for repository tests.
/// </summary>
[Collection(nameof(PostgresCollection))]
public abstract class PostgresTestBase : IAsyncLifetime
{
    /// <summary>
    /// Testcontainer-backed fixture (connection string, lifecycle, etc.).
    /// </summary>
    protected readonly PostgresFixture Fixture;

    /// <summary>
    /// Connection options for PostgreSQL.
    /// </summary>
    internal readonly PostgresOptions PostgresOptions;

    /// <summary>
    /// Cancellation token source for tests.
    /// </summary>
    protected readonly CancellationTokenSource Cts;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostgresTestBase"/> class.
    /// </summary>
    protected PostgresTestBase(PostgresFixture fixture)
    {
        Fixture = fixture;
        PostgresOptions = Integration.Postgres.Tools.GetConfiguration(Fixture.ConnectionString);
        Cts = new CancellationTokenSource();
    }

    /// <summary>
    /// Creates a new EF Core context instance.
    /// Purpose: keep tests isolated from DI setup.
    /// </summary>
    internal PostgresContext CreateContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<PostgresContext>();
        var connectionString = ConnectionWrapper.Wrap(PostgresOptions);
        optionsBuilder.UseNpgsql(connectionString);
        return new PostgresContext(optionsBuilder.Options);
    }

    /// <summary>
    /// Creates a repository instance for the given context.
    /// </summary>
    internal static UserPostgresRepository CreateRepository(PostgresContext context) => new(context);

    /// <summary>
    /// Creates a developer profile repository instance for the given context.
    /// </summary>
    /// <param name="context">The database context to use.</param>
    /// <returns>A new developer profile repository instance.</returns>
    internal static DeveloperProfilePostgresRepository CreateProfileRepository(PostgresContext context) => new(context);

    /// <summary>
    /// Seeds a user using repository API.
    /// Purpose: keep tests independent from EF persistence model (entities).
    /// </summary>
    /// <param name="user">The user to seed.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The seeded user.</returns>
    internal async Task<User?> SeedUserAsync(User user, CancellationToken ct)
    {
        await using var context = CreateContext();
        var repository = CreateRepository(context);
        return await repository.CreateAsync(user, ct);
    }

    /// <summary>
    /// Seeds a developer profile using repository API.
    /// Purpose: keep tests independent from EF persistence model (entities).
    /// </summary>
    /// <param name="profile">The developer profile to seed.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The seeded developer profile.</returns>
    internal async Task<DeveloperProfile?> SeedProfileAsync(DeveloperProfile profile, CancellationToken ct)
    {
        await using var context = CreateContext();
        var repository = CreateProfileRepository(context);
        return await repository.CreateAsync(profile, ct);
    }

    /// <summary>
    /// Reads user from the database using a new context.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The user if found; otherwise, null.</returns>
    internal async Task<User?> GetFreshUserAsync(Guid id, CancellationToken ct)
    {
        await using var context = CreateContext();
        var repository = CreateRepository(context);
        return await repository.GetByIdAsync(id, ct);
    }

    /// <summary>
    /// Reads developer profile from the database using a new context.
    /// </summary>
    /// <param name="id">The profile ID.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The profile if found; otherwise, null.</returns>
    internal async Task<DeveloperProfile?> GetFreshProfileAsync(Guid id, CancellationToken ct)
    {
        await using var context = CreateContext();
        var repository = CreateProfileRepository(context);
        return await repository.GetByIdAsync(id, ct);
    }

    /// <summary>
    /// Initializes database schema and clears tables before each test.
    /// </summary>
    public virtual async Task InitializeAsync()
    {
        await using var context = CreateContext();
        await context.Database.MigrateAsync(Cts.Token);
        await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"users\", \"developer_profiles\" RESTART IDENTITY CASCADE;", Cts.Token);
    }

    /// <summary>
    /// Cancels test token source.
    /// </summary>
    public Task DisposeAsync()
        => Cts.CancelAsync();
}
