using AutoFixture;
using AutoFixture.AutoMoq;
using CVA.Application.ProfileService;
using CVA.Domain.Interfaces;
using CVA.Domain.Models;
using Moq;

namespace CVA.Tests.Unit.Application.Handlers;

/// <summary>
/// Unit tests for <see cref="UpdateProfileHeaderHandler"/>.
/// </summary>
[Trait(Layer.Application, Category.Handlers)]
public sealed class UpdateProfileHeaderHandlerTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IDeveloperProfileRepository> _repositoryMock;
    private readonly Mock<ICurrentUserAccessor> _userAccessorMock;
    private readonly UpdateProfileHeaderHandler _sut;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateProfileHeaderHandlerTests"/> class.
    /// </summary>
    public UpdateProfileHeaderHandlerTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _fixture.Customizations.Add(DeveloperProfileBuilder.Instance);

        _fixture.Register(() => DateOnly.FromDateTime(DateTime.Today));
        _fixture.Register(() => Url.From("https://example.com/" + Guid.NewGuid()));

        _fixture
            .Customize<UpdateProfileHeaderRequest>(composer =>
                composer.With(request => request.AvatarUrl, "https://example.com/avatar.png"));

        _repositoryMock = _fixture.Freeze<Mock<IDeveloperProfileRepository>>();
        _userAccessorMock = _fixture.Freeze<Mock<ICurrentUserAccessor>>();

        _sut = _fixture.Create<UpdateProfileHeaderHandler>();
    }

    /// <summary>
    /// Purpose: Verify profile header data is updated when profile exists.
    /// When: HandleAsync is called with a valid update command.
    /// Should: Persist the updated name and role fields.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldUpdateHeader_WhenProfileExists()
    {
        // Arrange
        var userId = _fixture.Create<Guid>();
        var profile = _fixture.Create<DeveloperProfile>();
        var request = _fixture.Create<UpdateProfileHeaderRequest>();
        var command = new UpdateProfileHeaderCommand(request);

        _userAccessorMock
            .Setup(accessor => accessor.UserId)
            .Returns(userId);
        _repositoryMock
            .Setup(repository => repository.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(profile);
        _repositoryMock
            .Setup(repository => repository.UpdateAsync(It.IsAny<DeveloperProfile>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(profile);

        // Act
        var result = await _sut.HandleAsync(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _repositoryMock
            .Verify(repository => repository.UpdateAsync(It.Is<DeveloperProfile>(developerProfile =>
                developerProfile.Name.FirstName == request.FirstName &&
                developerProfile.Name.LastName == request.LastName &&
                developerProfile.Role!.Value == request.Role), It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Purpose: Verify handler returns an error for missing profiles.
    /// When: HandleAsync is called for a non-existent profile.
    /// Should: Return a failure result with a not-found message.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldReturnFail_WhenProfileNotFound()
    {
        // Arrange
        var userId = _fixture.Create<Guid>();
        var request = _fixture.Create<UpdateProfileHeaderRequest>();
        var command = new UpdateProfileHeaderCommand(request);

        _userAccessorMock
            .Setup(accessor => accessor.UserId)
            .Returns(userId);
        _repositoryMock
            .Setup(repository => repository.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((DeveloperProfile?)null);

        // Act
        var result = await _sut.HandleAsync(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Profile not found.", result.Error?.Message);
    }
}