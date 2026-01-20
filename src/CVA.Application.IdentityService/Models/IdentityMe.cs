namespace CVA.Application.IdentityService;

/// <summary>
/// Represents minimal identity information about the currently authenticated user.
/// This model is NOT a public profile.
/// </summary>
/// <param name="UserId">Internal user identifier.</param>
/// <param name="Role">User role.</param>
/// <param name="Email">User email, if available.</param>
public sealed record IdentityMe(Guid UserId, string Role, string Email);