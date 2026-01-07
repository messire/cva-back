namespace CVA.Application.DeveloperProfiles;

/// <summary>
/// Provides access to the current authenticated user context.
/// </summary>
public interface ICurrentUserAccessor
{
    /// <summary>
    /// Gets the identifier of the current user.
    /// </summary>
    Guid UserId { get; }

    /// <summary>
    /// Indicates whether the current request is authenticated.
    /// </summary>
    bool IsAuthenticated { get; }
}