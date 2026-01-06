using CVA.Domain.Models;

namespace CVA.Tests.Unit.Domain.ValueObjects;

/// <summary>
/// Unit tests for basic value objects.
/// </summary>
[Trait(Layer.Domain, Category.ValueObjects)]
public class BasicValueObjectsTests
{
    /// <summary>
    /// Purpose: Verify PersonName creation and normalization.
    /// Should: Trim whitespace from first and last names.
    /// When: Names with extra spaces are provided.
    /// </summary>
    [Theory]
    [InlineData("  John  ", "  Doe  ", "John", "Doe")]
    public void PersonName_Should_Trim_Values(string firstName, string lastName, string expectedFirst, string expectedLast)
    {
        // Act
        var name = PersonName.From(firstName, lastName);

        // Assert
        Assert.Equal(expectedFirst, name.FirstName);
        Assert.Equal(expectedLast, name.LastName);
    }

    /// <summary>
    /// Purpose: Verify EmailAddress validation.
    /// Should: Throw ArgumentException for invalid emails.
    /// When: Email without '@' is provided.
    /// </summary>
    [Theory]
    [InlineData("invalid")]
    [InlineData("   ")]
    public void EmailAddress_Should_Throw_On_Invalid_Value(string value)
    {
        // Act & Assert
        Assert.ThrowsAny<ArgumentException>(() => EmailAddress.From(value));
    }

    /// <summary>
    /// Purpose: Verify Url validation and TryFrom behavior.
    /// Should: Return null for null or whitespace in TryFrom.
    /// When: Value is null or empty.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public void Url_TryFrom_Should_Return_Null_On_Empty_Value(string? value)
    {
        // Act
        var url = Url.TryFrom(value);

        // Assert
        Assert.Null(url);
    }

    /// <summary>
    /// Purpose: Verify Url validation in TryFrom.
    /// Should: Throw ArgumentException for invalid URLs.
    /// When: Value is a non-empty string but not a valid URL.
    /// </summary>
    [Fact]
    public void Url_TryFrom_Should_Throw_On_Invalid_Format()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Url.TryFrom("not-a-url"));
    }

    /// <summary>
    /// Purpose: Verify YearsOfExperience range validation.
    /// Should: Throw ArgumentOutOfRangeException for values outside 0-80.
    /// When: Value is -1 or 81.
    /// </summary>
    [Theory]
    [InlineData(-1)]
    [InlineData(81)]
    public void YearsOfExperience_Should_Throw_On_Invalid_Range(int value)
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => YearsOfExperience.From(value));
    }

    /// <summary>
    /// Purpose: Verify DateRange validation.
    /// Should: Throw ArgumentException if end date is before start date.
    /// When: End date is earlier than start date.
    /// </summary>
    [Fact]
    public void DateRange_Should_Throw_If_End_Before_Start()
    {
        // Arrange
        var start = new DateOnly(2023, 1, 1);
        var end = new DateOnly(2022, 1, 1);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => DateRange.From(start, end));
    }

    /// <summary>
    /// Purpose: Verify simple string wrappers (RoleTitle, CompanyName, etc).
    /// Should: Trim the input value and set the Value property.
    /// When: A string with leading/trailing spaces is provided.
    /// </summary>
    [Theory]
    [InlineData("  Dev  ")]
    public void StringWrappers_Should_Trim_And_Set_Value(string input)
    {
        // Act & Assert
        Assert.Equal("Dev", RoleTitle.From(input).Value);
        Assert.Equal("Dev", CompanyName.From(input).Value);
        Assert.Equal("Dev", SkillTag.From(input).Value);
        Assert.Equal("Dev", TechTag.From(input).Value);
        Assert.Equal("Dev", ProjectName.From(input).Value);
    }

    /// <summary>
    /// Purpose: Verify optional string wrappers (ProfileSummary, ProjectDescription, etc).
    /// Should: Return null for empty input and trimmed value for non-empty.
    /// When: Various string inputs are provided to TryFrom.
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void OptionalStringWrappers_TryFrom_Should_Handle_Empty(string? input)
    {
        // Act & Assert
        Assert.Null(ProfileSummary.TryFrom(input));
        Assert.Null(ProjectDescription.TryFrom(input));
        Assert.Null(WorkDescription.TryFrom(input));
        Assert.Null(ProjectIcon.TryFrom(input));
    }

    /// <summary>
    /// Purpose: Verify Avatar creation.
    /// Should: Correcty wrap an image URL.
    /// When: A valid URL string is provided to TryFrom.
    /// </summary>
    [Fact]
    public void Avatar_TryFrom_Should_Wrap_Url()
    {
        // Arrange
        var url = "https://example.com/a.jpg";

        // Act
        var avatar = Avatar.TryFrom(url);

        // Assert
        Assert.NotNull(avatar);
        Assert.Equal(url, avatar!.ImageUrl.Value);
    }

    /// <summary>
    /// Purpose: Verify ContactInfo creation.
    /// Should: Initialize all properties correctly.
    /// When: Valid location, email and website are provided.
    /// </summary>
    [Theory, CvaAutoData]
    public void ContactInfo_Should_Initialize(Location location, EmailAddress email, Url website)
    {
        // Act
        var contact = ContactInfo.Create(location, email, website);

        // Assert
        Assert.Equal(location, contact.Location);
        Assert.Equal(email, contact.Email);
        Assert.Equal(website, contact.Website);
    }

    /// <summary>
    /// Purpose: Verify OpenToWorkStatus behavior.
    /// Should: Correcty wrap a boolean value.
    /// When: true or false is provided to constructor.
    /// </summary>
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void OpenToWorkStatus_Should_Wrap_Value(bool value)
    {
        // Act
        var status = new OpenToWorkStatus(value);

        // Assert
        Assert.Equal(value, status.Value);
    }

    /// <summary>
    /// Purpose: Verify ID value objects (DeveloperId, ProjectId, WorkExperienceId).
    /// Should: Correcty wrap a Guid value.
    /// When: A Guid is provided to constructor.
    /// </summary>
    [Theory, CvaAutoData]
    public void IdWrappers_Should_Wrap_Guid(Guid id)
    {
        // Act & Assert
        Assert.Equal(id, new DeveloperId(id).Value);
        Assert.Equal(id, new ProjectId(id).Value);
        Assert.Equal(id, new WorkExperienceId(id).Value);
    }

    /// <summary>
    /// Purpose: Verify EmailAddress.From validation and normalization.
    /// Should: Trim value and throw exception for invalid format or empty input.
    /// When: Valid email, invalid email, or empty string is provided.
    /// </summary>
    [Theory]
    [InlineData("  test@example.com  ", "test@example.com")]
    public void EmailAddress_From_Should_Normalize_And_Validate(string input, string expected)
    {
        // Act
        var email = EmailAddress.From(input);

        // Assert
        Assert.Equal(expected, email.Value);
    }

    /// <summary>
    /// Purpose: Verify From method for string wrappers.
    /// Should: Throw ArgumentException when input is null, empty or whitespace.
    /// When: Invalid string input is provided to From.
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void StringWrappers_From_Should_Throw_On_Empty(string? input)
    {
        // Act & Assert
        Assert.ThrowsAny<ArgumentException>(() => ProfileSummary.From(input!));
        Assert.ThrowsAny<ArgumentException>(() => ProjectDescription.From(input!));
        Assert.ThrowsAny<ArgumentException>(() => WorkDescription.From(input!));
        Assert.ThrowsAny<ArgumentException>(() => RoleTitle.From(input!));
        Assert.ThrowsAny<ArgumentException>(() => EmailAddress.From(input!));
    }

    /// <summary>
    /// Purpose: Verify ProjectIcon.From behavior.
    /// Should: Correcty wrap a URL or throw for invalid input.
    /// When: Valid URL or invalid/empty string is provided.
    /// </summary>
    [Theory]
    [InlineData("https://example.com/icon.png", "https://example.com/icon.png")]
    public void ProjectIcon_From_Should_Work_On_Valid_Url(string input, string expected)
    {
        // Act
        var icon = ProjectIcon.From(input);

        // Assert
        Assert.Equal(expected, icon.ImageUrl.Value);
    }

    [Theory]
    [InlineData("not-a-url")]
    [InlineData("   ")]
    public void ProjectIcon_From_Should_Throw_On_Invalid_Input(string input)
    {
        // Act & Assert
        Assert.ThrowsAny<ArgumentException>(() => ProjectIcon.From(input));
    }

    /// <summary>
    /// Purpose: Verify WorkDescription length validation.
    /// Should: Throw ArgumentOutOfRangeException if length exceeds 1000.
    /// When: A string longer than MaxLength is provided.
    /// </summary>
    [Fact]
    public void WorkDescription_Should_Throw_On_Exceeding_MaxLength()
    {
        // Arrange
        var longDescription = new string('a', WorkDescription.MaxLength + 1);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => WorkDescription.From(longDescription));
    }

    /// <summary>
    /// Purpose: Verify ProjectLink creation.
    /// Should: Correcty wrap a URL or null.
    /// When: Valid URL or null/empty string is provided.
    /// </summary>
    [Theory]
    [InlineData("https://github.com", "https://github.com")]
    [InlineData(null, null)]
    [InlineData("", null)]
    [InlineData("  ", null)]
    public void ProjectLink_Should_Wrap_Url(string? input, string? expected)
    {
        // Act
        var link = ProjectLink.From(input);

        // Assert
        Assert.Equal(expected, link.Value?.Value);
    }
}
