namespace CVA.Infrastructure.Postgres;

/// <summary>
/// Configures the entity mapping for the <see cref="RefreshTokenEntity"/> class within the database context.
/// </summary>
internal sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshTokenEntity>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
    {
        builder.ToTable("refresh_tokens");
        builder.HasKey(entity => entity.Id);

        builder.HasIndex(entity => entity.UserId);
        builder.HasIndex(entity => entity.TokenHash).IsUnique();

        builder.SetProperty(entity => entity.Id);
        builder.SetProperty(entity => entity.UserId);
        builder.SetProperty(entity => entity.TokenHash, 64).IsRequired();
        builder.SetProperty(entity => entity.ExpiresAt).IsRequired();
        builder.SetProperty(entity => entity.CreatedAt).IsRequired();
        builder.SetProperty(entity => entity.RevokedAt);
        builder.SetProperty(entity => entity.ReplacedByTokenHash, 64);

        builder.HasOne<UserEntity>()
            .WithMany()
            .HasForeignKey(entity => entity.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}