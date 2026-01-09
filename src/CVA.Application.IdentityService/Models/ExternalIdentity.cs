namespace CVA.Application.IdentityService;

/// <summary>
/// Represents verified external identity from an authentication provider.
/// </summary>
/// <param name="Provider">External provider name (for example: Google).</param>
/// <param name="Subject">Subject identifier provided by the external provider. For Google, this is the "sub" claim.</param>
/// <param name="Email">Email address provided by the external provider.</param>
public sealed record ExternalIdentity(string Provider, string Subject, string Email);