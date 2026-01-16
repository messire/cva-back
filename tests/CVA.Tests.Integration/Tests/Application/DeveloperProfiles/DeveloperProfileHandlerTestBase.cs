using CVA.Application.ProfileService;
using CVA.Infrastructure.Postgres;
using CVA.Tests.Integration.Tests.Infrastructure.Postgres;
using Moq;

namespace CVA.Tests.Integration.Tests.Application.DeveloperProfiles;

/// <summary>
/// Base class for Developer Profile handler integration tests.
/// Provides database context and mocked user identity.
/// </summary>
public abstract class DeveloperProfileHandlerTestBase : PostgresTestBase
{
    /// <summary>
    /// Mock implementation of the <see cref="ICurrentUserAccessor"/>.
    /// </summary>
    protected readonly Mock<ICurrentUserAccessor> UserAccessorMock = new();

    /// <summary>
    /// Represents the unique identifier of the current user in the context of integration tests.
    /// Used to simulate and mock the authenticated user's identity for testing purposes.
    /// </summary>
    protected Guid CurrentUserId = Guid.NewGuid();

    /// <summary>
    /// Initializes a new instance of the <see cref="DeveloperProfileHandlerTestBase"/> class.
    /// </summary>
    /// <param name="fixture">Shared Postgres fixture for integration tests.</param>
    protected DeveloperProfileHandlerTestBase(PostgresFixture fixture) : base(fixture)
    {
        UserAccessorMock.Setup(accessor => accessor.UserId).Returns(() => CurrentUserId);
        UserAccessorMock.Setup(accessor => accessor.IsAuthenticated).Returns(true);
    }

    /// <summary>
    /// Creates a repository instance using a fresh context.
    /// </summary>
    internal DeveloperProfilePostgresRepository CreateRepository()
        => CreateProfileRepository(CreateContext());
}
