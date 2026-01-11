using CVA.Application.IdentityService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CVA.Presentation.Auth;

/// <summary>
/// Orchestrates Google OAuth redirect-based authentication flow.
/// </summary>
/// <param name="httpContextAccessor">HTTP context accessor used to build absolute callback URL.</param>
/// <param name="authorizeUrlBuilder">Google authorize URL builder.</param>
/// <param name="stateStore">OAuth state store.</param>
/// <param name="googleOAuthClient">Google OAuth client for code exchange.</param>
/// <param name="identityService">Identity service issuing application JWT tokens.</param>
/// <param name="oneTimeCodeStore">One-time code store.</param>
internal sealed class GoogleOAuthFlow(
    IHttpContextAccessor httpContextAccessor,
    GoogleAuthorizeUrlBuilder authorizeUrlBuilder,
    IOAuthStateStore stateStore,
    IGoogleOAuthClient googleOAuthClient,
    IIdentityService identityService,
    IOneTimeCodeStore oneTimeCodeStore) : IGoogleOAuthFlow
{
    private static readonly TimeSpan StateTtl = TimeSpan.FromMinutes(10);
    private static readonly TimeSpan ExchangeCodeTtl = TimeSpan.FromMinutes(2);

    /// <inheritdoc />
    public IActionResult Start(string? returnUrl)
    {
        var validateResult = ValidateReturnUrl(returnUrl);
        if (!validateResult.isValid)
        {
            return validateResult.result!;
        }

        var http = httpContextAccessor.HttpContext;
        if (http is null)
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        var state = stateStore.Create(returnUrl!, StateTtl);
        var callbackUrl = BuildAbsoluteCallbackUrl(http);
        var googleUrl = authorizeUrlBuilder.Build(callbackUrl, state);

        return new RedirectResult(googleUrl, permanent: false);
    }

    /// <inheritdoc />
    public async Task<IActionResult> Callback(string? code, string? state, string? error, string? errorDescription, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(state))
        {
            return new BadRequestObjectResult(new { message = "state is required." });
        }

        if (!stateStore.TryConsume(state, out var returnUrl) || string.IsNullOrWhiteSpace(returnUrl))
        {
            return new BadRequestObjectResult(new { message = "Invalid or expired state." });
        }

        if (!string.IsNullOrWhiteSpace(error))
        {
            var url = AppendQuery(returnUrl, "error", error);

            if (!string.IsNullOrWhiteSpace(errorDescription))
            {
                url = AppendQuery(url, "errorDescription", errorDescription);
            }

            return new RedirectResult(url, permanent: false);
        }

        if (string.IsNullOrWhiteSpace(code))
        {
            return new RedirectResult(AppendQuery(returnUrl, "error", "missing_code"), permanent: false);
        }

        var http = httpContextAccessor.HttpContext;
        if (http is null)
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        var callbackUrl = BuildAbsoluteCallbackUrl(http);

        var googleIdToken = await googleOAuthClient.ExchangeCodeForIdTokenAsync(code, callbackUrl, ct);
        var tokenPair = await identityService.SignInWithGoogleAsync(googleIdToken, ct);
        var oneTimeCode = oneTimeCodeStore.Create(tokenPair, ExchangeCodeTtl);

        return new RedirectResult(AppendQuery(returnUrl, "code", oneTimeCode), permanent: false);
    }

    private static (bool isValid, BadRequestObjectResult? result) ValidateReturnUrl(string? returnUrl)
    {
        if (string.IsNullOrWhiteSpace(returnUrl))
        {
            var error = new
            {
                message = "returnUrl is required."
            };
            return (false, new BadRequestObjectResult(error));
        }

        if (!IsValidAbsoluteHttpUrl(returnUrl))
        {
            var error = new
            {
                message = "returnUrl must be an absolute http/https URL."
            };
            return (false, new BadRequestObjectResult(error));
        }
        return (true, null);
    }
    
    private static bool IsValidAbsoluteHttpUrl(string url)
        => Uri.TryCreate(url, UriKind.Absolute, out var uri)
           && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);

    private static string BuildAbsoluteCallbackUrl(HttpContext http)
    {
        var req = http.Request;
        var pathBase = req.PathBase.HasValue ? req.PathBase.Value : string.Empty;
        return $"{req.Scheme}://{req.Host}{pathBase}/api/auth/google/callback";
    }

    private static string AppendQuery(string url, string key, string value)
    {
        var separator = url.Contains('?', StringComparison.Ordinal) ? "&" : "?";
        return $"{url}{separator}{Uri.EscapeDataString(key)}={Uri.EscapeDataString(value)}";
    }
}