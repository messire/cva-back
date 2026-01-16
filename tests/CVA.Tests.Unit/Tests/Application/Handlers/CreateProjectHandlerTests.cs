using AutoFixture;
using AutoFixture.AutoMoq;
using CVA.Application.ProfileService;
using CVA.Domain.Interfaces;
using CVA.Domain.Models;
using Moq;

namespace CVA.Tests.Unit.Application.Handlers;

/// <summary>
/// Unit tests for <see cref="CreateProjectHandler"/>.
/// </summary>
[Trait(Layer.Application, Category.Handlers)]
public sealed class CreateProjectHandlerTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IDeveloperProfileRepository> _repositoryMock;
    private readonly Mock<ICurrentUserAccessor> _userAccessorMock;
    private readonly CreateProjectHandler _sut;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateProjectHandlerTests"/> class.
    /// </summary>
    public CreateProjectHandlerTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _fixture.Customizations.Add(DeveloperProfileBuilder.Instance);
        
        _fixture.Register(() => DateOnly.FromDateTime(DateTime.Today));
        _fixture.Register(() => Url.From("https://example.com/" + Guid.NewGuid()));
        
        _fixture.Customize<UpsertProjectRequest>(c => c
            .With(x => x.LinkUrl, "https://example.com/project")
            .With(x => x.IconUrl, "https://example.com/icon.png"));
            
        _repositoryMock = _fixture.Freeze<Mock<IDeveloperProfileRepository>>();
        _userAccessorMock = _fixture.Freeze<Mock<ICurrentUserAccessor>>();
        
        _sut = _fixture.Create<CreateProjectHandler>();
    }

    /// <summary>
    /// Verifies that a project is successfully added when the profile exists.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldAddProject_WhenProfileExists()
    {
        // Arrange
        var userId = _fixture.Create<Guid>();
        var profile = _fixture.Create<DeveloperProfile>();
        var request = _fixture.Create<UpsertProjectRequest>();
        var command = new CreateProjectCommand(request);

        _userAccessorMock.Setup(x => x.UserId).Returns(userId);
        _repositoryMock.Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(profile);
        _repositoryMock.Setup(x => x.UpdateAsync(It.IsAny<DeveloperProfile>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(profile);

        // Act
        var result = await _sut.HandleAsync(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _repositoryMock.Verify(x => x.UpdateAsync(It.Is<DeveloperProfile>(p => 
            p.Projects.Any(pr => pr.Name.Value == request.Name)), It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Verifies that a failure result is returned when the profile is not found.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldReturnFail_WhenProfileNotFound()
    {
        // Arrange
        var userId = _fixture.Create<Guid>();
        var request = _fixture.Create<UpsertProjectRequest>();
        var command = new CreateProjectCommand(request);

        _userAccessorMock.Setup(x => x.UserId).Returns(userId);
        _repositoryMock.Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((DeveloperProfile?)null);

        // Act
        var result = await _sut.HandleAsync(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Profile not found.", result.Error?.Message);
    }
}
