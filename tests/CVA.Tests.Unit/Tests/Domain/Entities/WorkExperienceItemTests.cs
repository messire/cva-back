using CVA.Domain.Models;

namespace CVA.Tests.Unit.Domain.Entities;

/// <summary>
/// Unit tests for the <see cref="WorkExperienceItem"/> entity.
/// </summary>
[Trait(Layer.Domain, Category.Entities)]
public class WorkExperienceItemTests
{
    /// <summary>
    /// Purpose: Verify that Create method correctly initializes a new work experience item.
    /// Should: Set all properties correctly from provided arguments.
    /// When: Valid arguments are provided.
    /// </summary>
    [Theory, CvaAutoData]
    public void Create_Should_Initialize_WorkExperienceItem(
        WorkExperienceId id, CompanyName company, Location location, RoleTitle role, WorkDescription description, DateRange period, TechTag[] techStack)
    {
        // Act
        var work = WorkExperienceItem.Create(id, company, location, role, description, period, techStack);

        // Assert
        Assert.Equal(id, work.Id);
        Assert.Equal(company, work.Company);
        Assert.Equal(location, work.Location);
        Assert.Equal(role, work.Role);
        Assert.Equal(description, work.Description);
        Assert.Equal(period, work.Period);
        Assert.Equal(techStack.Length, work.TechStack.Count);
    }

    /// <summary>
    /// Purpose: Verify that Update method correctly updates work experience properties.
    /// Should: Update all properties except Id.
    /// When: Valid arguments are provided to Update.
    /// </summary>
    [Theory, CvaAutoData]
    public void Update_Should_Update_Properties(
        WorkExperienceItem work, CompanyName newCompany, Location newLocation, RoleTitle newRole, WorkDescription newDescription, DateRange newPeriod, TechTag[] newTechStack)
    {
        // Act
        work.Update(newCompany, newLocation, newRole, newDescription, newPeriod, newTechStack);

        // Assert
        Assert.Equal(newCompany, work.Company);
        Assert.Equal(newLocation, work.Location);
        Assert.Equal(newRole, work.Role);
        Assert.Equal(newDescription, work.Description);
        Assert.Equal(newPeriod, work.Period);
        Assert.Equal(newTechStack.Length, work.TechStack.Count);
    }

    /// <summary>
    /// Purpose: Ensure that tech stack is normalized (duplicates removed, nulls filtered) during reconstruction from persistence.
    /// Should: Contain only unique, non-null tech tags.
    /// When: Tech stack with duplicates and nulls is provided to FromPersistence.
    /// </summary>
    [Fact]
    public void FromPersistence_Should_Normalize_TechStack()
    {
        // Arrange
        var id = new WorkExperienceId(Guid.NewGuid());
        var company = CompanyName.From("Company");
        var role = RoleTitle.From("Developer");
        var period = DateRange.From(new DateOnly(2020, 1, 1), null);
        var tag1 = TechTag.From("C#");
        var techStack = new[] { tag1, tag1, null };

        // Act
        var work = WorkExperienceItem.FromPersistence(id, company, null, role, null, period, techStack!);

        // Assert
        Assert.Single(work.TechStack);
        Assert.Equal(tag1, work.TechStack[0]);
    }

    /// <summary>
    /// Purpose: Verify validation in Create method.
    /// Should: Throw ArgumentException for empty Guid.
    /// When: WorkExperienceId with Guid.Empty is provided.
    /// </summary>
    [Fact]
    public void Create_Should_Throw_On_Empty_Id()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => WorkExperienceItem.Create(
            new WorkExperienceId(Guid.Empty), 
            CompanyName.From("C"), 
            null, 
            RoleTitle.From("R"), 
            null, 
            DateRange.From(new DateOnly(2020,1,1), null), 
            []));
    }
}
