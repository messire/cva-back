using CVA.Domain.Models;

namespace CVA.Tests.Unit.Domain.Models;

/// <summary>
/// Unit tests for the <see cref="User"/> domain model.
/// </summary>
[Trait(Layer.Domain, Category.Models)]
public class UserTests
{
    /// <summary>
    /// Purpose: Verify that Create method correctly initializes a new user.
    /// Should: Set all properties correctly from provided arguments.
    /// When: Valid name, surname and email are provided.
    /// </summary>
    [Theory, CvaAutoData]
    public void Create_Should_Initialize_User(string name, string surname)
    {
        // Arrange
        var email = "test@example.com";

        // Act
        var user = User.Create(name, surname, email);

        // Assert
        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.Equal(name, user.Name);
        Assert.Equal(surname, user.Surname);
        Assert.Equal(email, user.Email.Value);
    }

    /// <summary>
    /// Purpose: Verify that FromPersistence method correctly reconstructs a user aggregate.
    /// Should: Restore all properties, including optional fields and collections.
    /// When: Valid state data is provided from persistence layer.
    /// </summary>
    [Theory, CvaAutoData]
    public void FromPersistence_Should_Reconstruct_User(
        Guid id, string name, string surname, string phone, DateOnly birthday, string summary)
    {
        // Arrange
        var email = "test@example.com";
        var photo = "https://example.com/photo.jpg";
        var skills = new[] { "C#", "SQL" };
        var works = new[] { Work.Create("Company") };

        // Act
        var user = User.FromPersistence(id, name, surname, email, phone, photo, birthday, summary, skills, works);

        // Assert
        Assert.Equal(id, user.Id);
        Assert.Equal(name, user.Name);
        Assert.Equal(surname, user.Surname);
        Assert.Equal(email, user.Email.Value);
        Assert.Equal(phone, user.Phone);
        Assert.Equal(photo, user.Photo);
        Assert.Equal(birthday, user.Birthday);
        Assert.Equal(summary, user.SummaryInfo);
        Assert.Equal(skills, user.Skills);
        Assert.Equal(works, user.WorkExperience);
    }

    /// <summary>
    /// Purpose: Ensure that user cannot be reconstructed with an empty ID.
    /// Should: Throw DomainValidationException with appropriate message.
    /// When: Empty Guid is passed to FromPersistence.
    /// </summary>
    [Fact]
    public void FromPersistence_Should_Throw_When_Id_Is_Empty()
    {
        // Act & Assert
        var exception = Assert.Throws<DomainValidationException>(() => 
            User.FromPersistence(Guid.Empty, "N", "S", "e@e.com", null, null, null, null, null, null));
        Assert.Equal("Id must not be empty.", exception.Message);
    }

    /// <summary>
    /// Purpose: Verify validation logic in ChangeName method.
    /// Should: Throw DomainValidationException when name is null, empty or whitespace.
    /// When: Invalid name string is provided.
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ChangeName_Should_Throw_When_Name_Is_Invalid(string? invalidName)
    {
        // Arrange
        var user = User.Create("Name", "Surname", "email@example.com");

        // Act & Assert
        var exception = Assert.Throws<DomainValidationException>(() => user.ChangeName(invalidName!, "Surname"));
        Assert.Contains("name", exception.Message);
    }

    /// <summary>
    /// Purpose: Verify that skills are correctly normalized during replacement.
    /// Should: Trim whitespace from each skill in the collection.
    /// When: Skills with leading/trailing spaces are provided.
    /// </summary>
    [Fact]
    public void ReplaceSkills_Should_Normalize_Skills()
    {
        // Arrange
        var user = User.Create("N", "S", "e@e.com");
        var skills = new[] { "  C#  ", " .NET " };

        // Act
        user.ReplaceSkills(skills);

        // Assert
        Assert.Equal("C#", user.Skills[0]);
        Assert.Equal(".NET", user.Skills[1]);
    }

    /// <summary>
    /// Purpose: Verify adding work experience to the user.
    /// Should: Increase the collection size and contain the added item.
    /// When: A valid Work instance is provided.
    /// </summary>
    [Fact]
    public void AddWork_Should_Add_To_Collection()
    {
        // Arrange
        var user = User.Create("N", "S", "e@e.com");
        var work = Work.Create("Company");

        // Act
        user.AddWork(work);

        // Assert
        Assert.Contains(work, user.WorkExperience);
    }

    /// <summary>
    /// Purpose: Verify removing work experience based on a predicate.
    /// Should: Remove all matching entries and return the count of removed items.
    /// When: Predicate matches specific work entries.
    /// </summary>
    [Fact]
    public void RemoveWork_Should_Remove_Matching_Entries()
    {
        // Arrange
        var user = User.Create("N", "S", "e@e.com");
        var work1 = Work.Create("C1");
        var work2 = Work.Create("C2");
        user.AddWork(work1);
        user.AddWork(work2);

        // Act
        var removedCount = user.RemoveWork(w => w.CompanyName == "C1");

        // Assert
        Assert.Equal(1, removedCount);
        Assert.Single(user.WorkExperience);
        Assert.DoesNotContain(work1, user.WorkExperience);
    }
}
