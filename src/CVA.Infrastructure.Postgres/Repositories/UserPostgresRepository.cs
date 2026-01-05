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
        var entity = await context.Users
            .Include(userEntity => userEntity.WorkExperience)
            .FirstOrDefaultAsync(userEntity => userEntity.Id == id, ct);

        return entity?.ToDomain();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken ct)
    {
        var entities = await context.Users
            .Include(userEntity => userEntity.WorkExperience)
            .ToListAsync(ct);

        return entities.Select(userEntity => userEntity.ToDomain());
    }

    /// <inheritdoc />
    public async Task<User?> UpdateAsync(User updatedUser, CancellationToken ct)
    {
        var entity = await context.Users
            .Include(userEntity => userEntity.WorkExperience)
            .FirstOrDefaultAsync(userEntity => userEntity.Id == updatedUser.Id, ct);

        if (entity is null) return null;

        entity.Name = updatedUser.Name;
        entity.Surname = updatedUser.Surname;
        entity.Email = updatedUser.Email;
        entity.Phone = updatedUser.Phone;
        entity.Photo = updatedUser.Photo;
        entity.Birthday = updatedUser.Birthday;
        entity.SummaryInfo = updatedUser.SummaryInfo;
        entity.Skills = updatedUser.Skills.ToList();
        entity.WorkExperience.Clear();
        entity.WorkExperience.AddRange(updatedUser.WorkExperience.Select(work => work.ToEntity()));

        await context.SaveChangesAsync(ct);
        return entity.ToDomain();
    }

    /// <inheritdoc />
    public async Task<User?> DeleteAsync(Guid id, CancellationToken ct)
    {
        var entity = await context.Users.FirstOrDefaultAsync(userEntity => userEntity.Id == id, ct);
        if (entity is null) return null;

        context.Users.Remove(entity);
        await context.SaveChangesAsync(ct);
        return entity.ToDomain();
    }
}