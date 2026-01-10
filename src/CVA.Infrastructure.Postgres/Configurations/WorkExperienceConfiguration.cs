namespace CVA.Infrastructure.Postgres;

/// <summary>
/// Configures the entity mapping for the <see cref="WorkExperienceEntity"/> class within the database context.
/// </summary>
internal sealed class WorkExperienceConfiguration : IEntityTypeConfiguration<WorkExperienceEntity>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<WorkExperienceEntity> builder)
    {
        builder.ToTable("work_experiences");
        builder.HasKey(entity => entity.Id);

        builder.SetProperty(entity => entity.Id).ValueGeneratedNever();
        builder.SetProperty(entity => entity.DeveloperProfileId).IsRequired();
        builder.SetProperty(entity => entity.Company, maxLength: 200).IsRequired();
        builder.SetProperty(entity => entity.Role, maxLength: 150).IsRequired();
        builder.SetProperty(entity => entity.Description);
        builder.SetProperty(entity => entity.StartDate).HasColumnType(DatabaseTypes.DateOnly);
        builder.SetProperty(entity => entity.EndDate).HasColumnType(DatabaseTypes.DateOnly);
        builder.SetProperty(entity => entity.TechStack).HasColumnType(DatabaseTypes.TextArray);

        builder.OwnsOne(entity => entity.Location, ConfigureLocation);
    }

    private static void ConfigureLocation(OwnedNavigationBuilder<WorkExperienceEntity, LocationEntity> location)
    {
        location.SetProperty(entity => entity.City, maxLength: 100);
        location.SetProperty(entity => entity.Country, maxLength: 100);
    }
}