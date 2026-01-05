namespace CVA.Infrastructure.Postgres;

/// <summary>
/// Provides configuration for the <see cref="Work"/> entity as an owned collection of the <see cref="User"/>.
/// </summary>
internal static class WorkConfiguration
{
    /// <summary>
    /// Configures the mapping for the Work entity when it is owned by a User.
    /// </summary>
    /// <param name="builder">The builder used to configure the owned navigation.</param>
    public static void Configure(OwnedNavigationBuilder<UserEntity, WorkEntity> builder)
    {
        builder.ToTable("works");

        builder.WithOwner()
            .HasForeignKey("user_id"); 

        // shadow PK to keep EF tracking stable without polluting the domain model
        builder.Property<Guid>("id");
        builder.HasKey("id");
        
        builder.Property(work => work.CompanyName)
            .HasColumnName(nameof(Work.CompanyName).ToSnakeCase())
            .HasMaxLength(100);

        builder.Property(work => work.Role)
            .HasColumnName(nameof(Work.Role).ToSnakeCase())
            .HasMaxLength(100);

        builder.Property(work => work.Description)
            .HasColumnName(nameof(Work.Description).ToSnakeCase());

        builder.Property(work => work.Location)
            .HasColumnName(nameof(Work.Location).ToSnakeCase())
            .HasMaxLength(200);

        builder.Property(work => work.StartDate)
            .HasColumnName(nameof(Work.StartDate).ToSnakeCase())
            .HasColumnType(DatabaseTypes.DateOnly);

        builder.Property(work => work.EndDate)
            .HasColumnName(nameof(Work.EndDate).ToSnakeCase())
            .HasColumnType(DatabaseTypes.DateOnly);

        builder.Property(work => work.Achievements)
            .HasColumnName(nameof(Work.Achievements).ToSnakeCase())
            .HasColumnType(DatabaseTypes.TextArray);

        builder.Property(work => work.TechStack)
            .HasColumnName(nameof(Work.TechStack).ToSnakeCase())
            .HasColumnType(DatabaseTypes.TextArray);
    }
}