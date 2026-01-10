using CVA.Domain.Models;

namespace CVA.Tests.Unit.Domain.Entities;

/// <summary>
/// Unit tests for the <see cref="ProjectItem"/> entity.
/// </summary>
[Trait(Layer.Domain, Category.Entities)]
public class ProjectItemTests
{
    /// <summary>
    /// Purpose: Verify that Create method correctly initializes a new project item.
    /// Should: Set all properties correctly from provided arguments.
    /// When: Valid arguments are provided.
    /// </summary>
    [Theory, CvaAutoData]
    public void Create_Should_Initialize_ProjectItem(
        ProjectId id, ProjectName name, ProjectDescription description, ProjectIcon icon, ProjectLink link, TechTag[] techStack)
    {
        // Act
        var project = ProjectItem.Create(id, name, description, icon, link, techStack);

        // Assert
        Assert.Equal(id, project.Id);
        Assert.Equal(name, project.Name);
        Assert.Equal(description, project.Description);
        Assert.Equal(icon, project.Icon);
        Assert.Equal(link, project.Link);
        Assert.Equal(techStack.Length, project.TechStack.Count);
    }

    /// <summary>
    /// Purpose: Verify that Update method correctly updates project item properties.
    /// Should: Update all properties except Id.
    /// When: Valid arguments are provided to Update.
    /// </summary>
    [Theory, CvaAutoData]
    public void Update_Should_Update_Properties(
        ProjectItem project, ProjectName newName, ProjectDescription newDescription, ProjectIcon newIcon, ProjectLink newLink, TechTag[] newTechStack)
    {
        // Act
        project.Update(newName, newDescription, newIcon, newLink, newTechStack);

        // Assert
        Assert.Equal(newName, project.Name);
        Assert.Equal(newDescription, project.Description);
        Assert.Equal(newIcon, project.Icon);
        Assert.Equal(newLink, project.Link);
        Assert.Equal(newTechStack.Length, project.TechStack.Count);
    }

    /// <summary>
    /// Purpose: Ensure that tech stack is normalized (duplicates removed, nulls filtered) during creation.
    /// Should: Contain only unique, non-null tech tags.
    /// When: Tech stack with duplicates and nulls is provided.
    /// </summary>
    [Fact]
    public void FromPersistence_Should_Normalize_TechStack()
    {
        // Arrange
        var id = new ProjectId(Guid.NewGuid());
        var name = ProjectName.From("Name");
        var link = ProjectLink.From("https://example.com");
        var tag1 = TechTag.From("C#");
        var techStack = new[] { tag1, tag1, null };

        // Act
        var project = ProjectItem.FromPersistence(id, name, null, null, link, techStack!);

        // Assert
        Assert.Single(project.TechStack);
        Assert.Equal(tag1, project.TechStack[0]);
    }

    /// <summary>
    /// Purpose: Verify validation in Create method.
    /// Should: Throw ArgumentException for empty Guid.
    /// When: ProjectId with Guid.Empty is provided.
    /// </summary>
    [Fact]
    public void Create_Should_Throw_On_Empty_Id()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => ProjectItem.Create(
            new ProjectId(Guid.Empty), 
            ProjectName.From("N"), 
            null, 
            null, 
            ProjectLink.From(null), 
            []));
    }
}
