namespace CVA.Infrastructure.Auth;

/// <summary>
/// Issues application access tokens (JWT) for authenticated users.
/// </summary>
public interface IAppTokenIssuer
{
    /// <summary>
    /// Issues an access token for a user.
    /// </summary>
    /// <param name="userId">User identifier (goes to <c>sub</c>).</param>
    /// <param name="role">User role (goes to <c>role</c>).</param>
    string Issue(Guid userId, string role);
}