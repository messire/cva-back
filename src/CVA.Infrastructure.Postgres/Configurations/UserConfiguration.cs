namespace CVA.Infrastructure.Postgres;

/// <summary>
/// Configures the entity mapping for the <c>User</c> class within the database context.
/// </summary>
internal class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("users");
        builder.HasKey(entity => entity.Id);
        builder.HasIndex(entity => entity.Email).IsUnique();
        builder.HasIndex(entity => entity.GoogleSubject).IsUnique();

        builder.SetProperty(entity => entity.Id);
        builder.SetProperty(entity => entity.Email, 320).IsRequired();
        builder.SetProperty(entity => entity.Role, 32).IsRequired();
        builder.SetProperty(entity => entity.GoogleSubject, 128).IsRequired();
        builder.Property(entity => entity.CreatedAt).IsRequired();
        builder.SetProperty(entity => entity.UpdatedAt).IsRequired();
    }
}