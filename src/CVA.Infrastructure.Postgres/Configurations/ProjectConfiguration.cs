namespace CVA.Infrastructure.Postgres;

/// <summary>
/// Configures the entity mapping for the <see cref="ProjectEntity"/> class within the database context.
/// </summary>
internal sealed class ProjectConfiguration : IEntityTypeConfiguration<ProjectEntity>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<ProjectEntity> builder)
    {
        builder.ToTable("projects");
        builder.HasKey(entity => entity.Id);

        builder.SetProperty(entity => entity.Id).ValueGeneratedNever();
        builder.SetProperty(entity => entity.DeveloperProfileId).IsRequired();
        builder.SetProperty(entity => entity.Name, maxLength: 200).IsRequired();
        builder.SetProperty(entity => entity.Description);
        builder.SetProperty(entity => entity.IconUrl, maxLength: 255);
        builder.SetProperty(entity => entity.LinkUrl, maxLength: 255);
        builder.SetProperty(entity => entity.TechStack).HasColumnType(DatabaseTypes.TextArray);
    }
}