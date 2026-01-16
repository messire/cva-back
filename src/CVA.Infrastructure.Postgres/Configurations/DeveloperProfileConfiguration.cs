namespace CVA.Infrastructure.Postgres;

/// <summary>
/// Configures the entity mapping for the <see cref="DeveloperProfileEntity"/> class within the database context.
/// </summary>
internal sealed class DeveloperProfileConfiguration : IEntityTypeConfiguration<DeveloperProfileEntity>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<DeveloperProfileEntity> builder)
    {
        builder.ToTable("developer_profiles");
        builder.HasKey(entity => entity.Id);

        builder.SetProperty(entity => entity.Id).ValueGeneratedNever();
        builder.SetProperty(entity => entity.FirstName, maxLength: 100).IsRequired();
        builder.SetProperty(entity => entity.LastName, maxLength: 100).IsRequired();
        builder.SetProperty(entity => entity.Role, maxLength: 150);
        builder.SetProperty(entity => entity.Summary);
        builder.SetProperty(entity => entity.AvatarUrl, maxLength: 255);
        builder.SetProperty(entity => entity.OpenToWork).HasDefaultValue(false);
        builder.SetProperty(entity => entity.Email, maxLength: 150).IsRequired();
        builder.SetProperty(entity => entity.Phone, maxLength: 30);
        builder.SetProperty(entity => entity.Website, maxLength: 255);
        builder.SetProperty(entity => entity.Skills).HasColumnType(DatabaseTypes.TextArray);
        builder.SetProperty(entity => entity.Verified).HasConversion<int>();
        builder.SetProperty(entity => entity.CreatedAt);
        builder.SetProperty(entity => entity.UpdatedAt);

        builder.OwnsOne(entity => entity.Location, ConfigureLocation);
        builder.OwnsOne(entity => entity.SocialLinks, SetupSocialLinks);

        builder.HasMany(entity => entity.Projects)
            .WithOne()
            .HasForeignKey(entity => entity.DeveloperProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(entity => entity.WorkExperience)
            .WithOne()
            .HasForeignKey(entity => entity.DeveloperProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureLocation(OwnedNavigationBuilder<DeveloperProfileEntity, LocationEntity> location)
    {
        location.SetProperty(entity => entity.City, maxLength: 100);
        location.SetProperty(entity => entity.Country, maxLength: 100);
    }

    private static void SetupSocialLinks(OwnedNavigationBuilder<DeveloperProfileEntity, SocialLinksEntity> social)
    {
        social.SetProperty(entity => entity.LinkedIn, maxLength: 255);
        social.SetProperty(entity => entity.GitHub, maxLength: 255);
        social.SetProperty(entity => entity.Twitter, maxLength: 255);
        social.SetProperty(entity => entity.Telegram, maxLength: 255);
    }
}