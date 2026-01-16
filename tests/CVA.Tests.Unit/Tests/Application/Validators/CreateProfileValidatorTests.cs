using AutoFixture;
using CVA.Application.ProfileService;
using CVA.Tests.Common;
using FluentValidation.TestHelper;

namespace CVA.Tests.Unit.Application.Validators;

/// <summary>
/// Unit tests for <see cref="CreateProfileValidator"/>.
/// </summary>
[Trait(Layer.Application, Category.Validators)]
public sealed class CreateProfileValidatorTests
{
    private readonly IFixture _fixture;
    private readonly CreateProfileValidator _validator;

    public CreateProfileValidatorTests()
    {
        _fixture = new Fixture();
        _validator = new CreateProfileValidator();
    }

    [Fact]
    public void Should_HaveError_When_FirstName_Is_Empty()
    {
        var command = CreateCommand(firstName: string.Empty);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Request.FirstName);
    }

    [Fact]
    public void Should_HaveError_When_Email_Is_Invalid()
    {
        var command = CreateCommand(email: "invalid-email");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Request.Email);
    }

    [Fact]
    public void Should_HaveError_When_AvatarUrl_Is_Invalid()
    {
        var command = CreateCommand(avatarUrl: "invalid-url");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Request.AvatarUrl);
    }

    [Fact]
    public void Should_Not_HaveError_When_Command_Is_Valid()
    {
        var command = CreateCommand();
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    private CreateProfileCommand CreateCommand(
        string firstName = "John",
        string lastName = "Doe",
        string email = "john.doe@example.com",
        string avatarUrl = "https://example.com/avatar.png")
    {
        var request = _fixture.Build<CreateProfileRequest>()
            .With(x => x.FirstName, firstName)
            .With(x => x.LastName, lastName)
            .With(x => x.Email, email)
            .With(x => x.AvatarUrl, avatarUrl)
            .With(x => x.Phone, "+1234567890")
            .With(x => x.Website, "https://example.com")
            .With(x => x.SocialLinks, new SocialLinksDto())
            .With(x => x.Location, new LocationDto { City = "New York", Country = "USA" })
            .Create();

        return new CreateProfileCommand(request);
    }
}
