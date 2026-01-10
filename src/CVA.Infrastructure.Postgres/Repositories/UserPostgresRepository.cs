namespace CVA.Infrastructure.Postgres;

/// <summary>
/// Provides an implementation of the <see cref="IUserRepository"/> interface for interacting with user data in a PostgreSQL database.
/// </summary>
internal class UserPostgresRepository(PostgresContext context) : IUserRepository
{
    /// <inheritdoc />
    public async Task<User?> CreateAsync(User user, CancellationToken ct)
    {
        var entity = user.ToEntity();
        await context.Users.AddAsync(entity, ct);
        await context.SaveChangesAsync(ct);
        return entity.ToDomain();
    }

    /// <inheritdoc />
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var entity = await context.Users.FirstOrDefaultAsync(userEntity => userEntity.Id == id, ct);
        return entity?.ToDomain();
    }

    public async Task<User?> GetByGoogleSubjectAsync(string googleSubject, CancellationToken ct)
    {
        var entity = await context.Users.FirstOrDefaultAsync(userEntity => userEntity.GoogleSubject == googleSubject, ct);
        return entity?.ToDomain();
    }

    public async Task<User?> UpdateRoleAsync(Guid id, string role, CancellationToken ct)
    {
        var entity = await context.Users
            .AsSplitQuery()
            .FirstOrDefaultAsync(userEntity => userEntity.Id == id, ct);
        if (entity is null) return null;

        entity.Role = role;
        entity.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync(ct);
        return entity.ToDomain();
    }
}