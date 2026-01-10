namespace CVA.Infrastructure.Postgres;

/// <inheritdoc />
internal sealed class PostgresContext(DbContextOptions<PostgresContext> options)
    : DbContext(options)
{
    /// <summary>
    /// Represents the collection of users in the database context.
    /// </summary>
    public DbSet<UserEntity> Users { get; set; } = null!;

    /// <summary>
    /// Represents the collection of refresh tokens in the database context.
    /// </summary>
    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; } = null!;

    /// <summary>
    /// Represents the collection of developer profiles in the database context.
    /// </summary>
    public DbSet<DeveloperProfileEntity> DeveloperProfiles { get; set; } = null!;

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostgresContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}