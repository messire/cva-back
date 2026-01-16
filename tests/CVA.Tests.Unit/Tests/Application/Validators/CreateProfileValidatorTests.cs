using AutoFixture;
using CVA.Application.ProfileService;

namespace CVA.Tests.Unit.Application.Validators;

/// <summary>
/// Unit tests for <see cref="CreateProfileValidator"/>.
/// </summary>
[Trait(Layer.Application, Category.Validators)]
public sealed class CreateProfileValidatorTests
{
    private readonly IFixture _fixture = new Fixture();
    private readonly CreateProfileValidator _validator = new ();

    /// <summary>
    /// Purpose: Verify first name is required when provided.
    /// When: FirstName is empty.
    /// Should: Return a validation error for FirstName.
    /// </summary>
    [Fact]
    public void Should_HaveError_When_FirstName_Is_Empty()
    {
        var command = CreateCommand(firstName: string.Empty);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(profileCommand => profileCommand.Request.FirstName);
    }

    /// <summary>
    /// Purpose: Verify email must be in a valid format.
    /// When: Email is not a valid address.
    /// Should: Return a validation error for Email.
    /// </summary>
    [Fact]
    public void Should_HaveError_When_Email_Is_Invalid()
    {
        var command = CreateCommand(email: "invalid-email");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(profileCommand => profileCommand.Request.Email);
    }

    /// <summary>
    /// Purpose: Verify avatar URL must be a valid absolute URI.
    /// When: AvatarUrl is not a valid URL.
    /// Should: Return a validation error for AvatarUrl.
    /// </summary>
    [Fact]
    public void Should_HaveError_When_AvatarUrl_Is_Invalid()
    {
        var command = CreateCommand(avatarUrl: "invalid-url");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(profileCommand => profileCommand.Request.AvatarUrl);
    }

    /// <summary>
    /// Purpose: Verify valid requests pass validation.
    /// When: The command contains a valid request.
    /// Should: Return no validation errors.
    /// </summary>
    [Fact]
    public void Should_Not_HaveError_When_Command_Is_Valid()
    {
        var command = CreateCommand();
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    /// <summary>
    /// Builds a create command with customizable fields.
    /// </summary>
    private CreateProfileCommand CreateCommand(
        string firstName = "John",
        string lastName = "Doe",
        string email = "john.doe@example.com",
        string avatarUrl = "https://example.com/avatar.png")
    {
        var request = _fixture.Build<CreateProfileRequest>()
            .With(profileRequest => profileRequest.FirstName, firstName)
            .With(profileRequest => profileRequest.LastName, lastName)
            .With(profileRequest => profileRequest.Email, email)
            .With(profileRequest => profileRequest.AvatarUrl, avatarUrl)
            .With(profileRequest => profileRequest.Phone, "+1234567890")
            .With(profileRequest => profileRequest.Website, "https://example.com")
            .With(profileRequest => profileRequest.SocialLinks, new SocialLinksDto())
            .With(profileRequest => profileRequest.Location, new LocationDto { City = "New York", Country = "USA" })
            .Create();

        return new CreateProfileCommand(request);
    }
}