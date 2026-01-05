using CVA.Domain.Models;

namespace CVA.Tests.Unit.Domain.Models;

/// <summary>
/// Unit tests for the <see cref="Work"/> domain model.
/// </summary>
[Trait(Layer.Domain, Category.Models)]
public class WorkTests
{
    /// <summary>
    /// Purpose: Verify that Create method correctly initializes work experience properties.
    /// Should: Set all fields correctly and preserve collections.
    /// When: Valid data for all properties is provided.
    /// </summary>
    [Fact]
    public void Create_Should_Initialize_Properties()
    {
        // Arrange
        var companyName = "Company";
        var role = "Developer";
        var startDate = new DateOnly(2020, 1, 1);
        var endDate = new DateOnly(2021, 1, 1);
        var description = "Description";
        var location = "Location";
        var achievements = new[] { "A1", "A2" };
        var techStack = new[] { "T1", "T2" };

        // Act
        var work = Work.Create(companyName, role, startDate, endDate, description, location, achievements, techStack);

        // Assert
        Assert.Equal(companyName, work.CompanyName);
        Assert.Equal(role, work.Role);
        Assert.Equal(startDate, work.StartDate);
        Assert.Equal(endDate, work.EndDate);
        Assert.Equal(description, work.Description);
        Assert.Equal(location, work.Location);
        Assert.Equal(achievements, work.Achievements);
        Assert.Equal(techStack, work.TechStack);
    }

    /// <summary>
    /// Purpose: Ensure validation of start and end dates.
    /// Should: Throw DomainValidationException when end date is before start date.
    /// When: Invalid date range is provided.
    /// </summary>
    [Fact]
    public void Update_Should_Throw_When_EndDate_Is_Before_StartDate()
    {
        // Arrange
        var startDate = new DateOnly(2021, 1, 1);
        var endDate = new DateOnly(2020, 1, 1);

        // Act & Assert
        var exception = Assert.Throws<DomainValidationException>(() => Work.Create(startDate: startDate, endDate: endDate));
        Assert.Equal("EndDate must be greater or equal to StartDate.", exception.Message);
    }

    /// <summary>
    /// Purpose: Verify normalization and filtering of list items in Work model.
    /// Should: Remove empty/whitespace items and trim valid ones.
    /// When: Collection with mixed valid and invalid strings is provided.
    /// </summary>
    [Fact]
    public void ReplaceList_Should_Filter_And_Trim_Elements()
    {
        // Arrange
        var techStack = new[] { "  C#  ", "", "   ", "  .NET  " };

        // Act
        var work = Work.Create(techStack: techStack);

        // Assert
        Assert.Equal(2, work.TechStack.Count);
        Assert.Equal("C#", work.TechStack[0]);
        Assert.Equal(".NET", work.TechStack[1]);
    }

    /// <summary>
    /// Purpose: Verify handling of null collections during update.
    /// Should: Clear the target collection instead of throwing or doing nothing.
    /// When: null is passed as a collection argument.
    /// </summary>
    [Fact]
    public void Update_Should_Handle_Null_Collections()
    {
        // Arrange
        var work = Work.Create(achievements: new[] { "Existing" });

        // Act
        work.Update(achievements: null);

        // Assert
        Assert.Empty(work.Achievements);
    }
}
