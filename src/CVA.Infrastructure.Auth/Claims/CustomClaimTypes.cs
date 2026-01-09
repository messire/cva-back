namespace CVA.Infrastructure.Auth;

/// <summary>
/// Defines custom JWT claim type names used by the application.
/// </summary>
internal static class CustomClaimTypes
{
    /// <summary>
    /// The subject claim. Represents the current user identifier.
    /// </summary>
    public const string Subject = "sub";

    /// <summary>
    /// The role claim. Represents the current user role.
    /// </summary>
    public const string Role = "role";
}