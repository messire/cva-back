using AutoFixture;
using CVA.Application.ProfileService;
using CVA.Domain.Interfaces;
using CVA.Domain.Models;
using Moq;

namespace CVA.Tests.Unit.Application.Handlers;

/// <summary>
/// Unit tests for <see cref="CreateProfileHandler"/>.
/// </summary>
[Trait(Layer.Application, Category.Handlers)]
public sealed class CreateProfileHandlerTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IDeveloperProfileRepository> _repositoryMock;
    private readonly Mock<ICurrentUserAccessor> _userAccessorMock;
    private readonly CreateProfileHandler _sut;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateProfileHandlerTests"/> class.
    /// </summary>
    public CreateProfileHandlerTests()
    {
        _fixture = new Fixture().Customize(new ApplicationTestCustomization());
        _fixture.Customizations.Add(DeveloperProfileBuilder.Instance);
        
        _fixture.Register(() => EmailAddress.From("test@example.com"));
        _fixture.Register(() => PhoneNumber.From("+1234567890"));
        _fixture.Register(() => Url.From("https://example.com"));

        _repositoryMock = _fixture.Freeze<Mock<IDeveloperProfileRepository>>();
        _userAccessorMock = _fixture.Freeze<Mock<ICurrentUserAccessor>>();
        
        _sut = _fixture.Create<CreateProfileHandler>();
    }

    /// <summary>
    /// Purpose: Verify a profile is created for an authenticated user without an existing profile.
    /// When: HandleAsync is called with a valid create command.
    /// Should: Persist a new profile tied to the current user.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldCreateProfile_WhenUserIsAuthenticatedAndProfileDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = _fixture.Build<CreateProfileRequest>()
            .With(x => x.Email, "test@example.com")
            .With(x => x.Phone, "+1234567890")
            .With(x => x.Website, "https://example.com")
            .With(x => x.AvatarUrl, "https://example.com/avatar.png")
            .With(x => x.SocialLinks, new SocialLinksDto())
            .Create();
        var command = new CreateProfileCommand(request);
        var profile = _fixture.Create<DeveloperProfile>();

        _userAccessorMock.Setup(x => x.IsAuthenticated).Returns(true);
        _userAccessorMock.Setup(x => x.UserId).Returns(userId);
        _repositoryMock.Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((DeveloperProfile?)null);
        _repositoryMock.Setup(x => x.CreateAsync(It.IsAny<DeveloperProfile>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(profile);

        // Act
        var result = await _sut.HandleAsync(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _repositoryMock.Verify(x => x.CreateAsync(It.Is<DeveloperProfile>(p => p.Id.Value == userId), It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Purpose: Verify handler rejects unauthenticated users.
    /// When: HandleAsync is called with a user that is not authenticated.
    /// Should: Return a failure result with an auth error message.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldReturnFailure_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var request = _fixture.Create<CreateProfileRequest>();
        var command = new CreateProfileCommand(request);

        _userAccessorMock.Setup(x => x.IsAuthenticated).Returns(false);

        // Act
        var result = await _sut.HandleAsync(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("User is not authenticated.", result.Error?.Message);
    }

    /// <summary>
    /// Purpose: Verify handler prevents duplicate profiles.
    /// When: HandleAsync is called for a user that already has a profile.
    /// Should: Return a conflict error.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldReturnConflict_WhenProfileAlreadyExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = _fixture.Create<CreateProfileRequest>();
        var command = new CreateProfileCommand(request);
        var profile = _fixture.Create<DeveloperProfile>();

        _userAccessorMock.Setup(x => x.IsAuthenticated).Returns(true);
        _userAccessorMock.Setup(x => x.UserId).Returns(userId);
        _repositoryMock.Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(profile);

        // Act
        var result = await _sut.HandleAsync(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Conflict", result.Error?.Code);
        Assert.Equal("Profile already exists.", result.Error?.Message);
    }
}
