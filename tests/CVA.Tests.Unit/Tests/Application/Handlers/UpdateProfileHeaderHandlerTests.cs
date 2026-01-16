using AutoFixture;
using AutoFixture.AutoMoq;
using CVA.Application.ProfileService;
using CVA.Domain.Interfaces;
using CVA.Domain.Models;
using Moq;

namespace CVA.Tests.Unit.Application.Handlers;

[Trait(Layer.Application, Category.Handlers)]
public sealed class UpdateProfileHeaderHandlerTests
{
    private readonly IFixture _fixture;
    private readonly Mock<IDeveloperProfileRepository> _repositoryMock;
    private readonly Mock<ICurrentUserAccessor> _userAccessorMock;
    private readonly UpdateProfileHeaderHandler _sut;

    public UpdateProfileHeaderHandlerTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _fixture.Customizations.Add(DeveloperProfileBuilder.Instance);
        
        _fixture.Register(() => DateOnly.FromDateTime(DateTime.Today));
        _fixture.Register(() => Url.From("https://example.com/" + Guid.NewGuid()));
        
        _fixture.Customize<UpdateProfileHeaderRequest>(c => c
            .With(x => x.AvatarUrl, "https://example.com/avatar.png"));
            
        _repositoryMock = _fixture.Freeze<Mock<IDeveloperProfileRepository>>();
        _userAccessorMock = _fixture.Freeze<Mock<ICurrentUserAccessor>>();
        
        _sut = _fixture.Create<UpdateProfileHeaderHandler>();
    }

    [Fact]
    public async Task HandleAsync_ShouldUpdateHeader_WhenProfileExists()
    {
        // Arrange
        var userId = _fixture.Create<Guid>();
        var profile = _fixture.Create<DeveloperProfile>();
        var request = _fixture.Create<UpdateProfileHeaderRequest>();
        var command = new UpdateProfileHeaderCommand(request);

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
            p.Name.FirstName == request.FirstName &&
            p.Name.LastName == request.LastName &&
            p.Role.Value == request.Role), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnFail_WhenProfileNotFound()
    {
        // Arrange
        var userId = _fixture.Create<Guid>();
        var request = _fixture.Create<UpdateProfileHeaderRequest>();
        var command = new UpdateProfileHeaderCommand(request);

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
