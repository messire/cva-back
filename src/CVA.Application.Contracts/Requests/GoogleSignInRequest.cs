namespace CVA.Application.Contracts;

/// <summary>
/// Request model for Google sign-in.
/// </summary>
public sealed class GoogleSignInRequest
{
    /// <summary>
    /// Google ID token received on the client side.
    /// </summary>
    public string IdToken { get; init; } = string.Empty;
}