namespace CVA.Application.Contracts;

/// <summary>
/// Request model for Google sign-in.
/// </summary>
/// <param name="IdToken">Google ID token received on the client side.</param>
public sealed record GoogleSignInRequest(string IdToken);