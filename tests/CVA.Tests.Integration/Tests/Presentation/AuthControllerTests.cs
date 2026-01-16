using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using CVA.Application.Contracts;
using CVA.Application.IdentityService;
using CVA.Infrastructure.Auth;
using CVA.Presentation.Auth;
using CVA.Presentation.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace CVA.Tests.Integration.Tests.Presentation;

/// <summary>
/// Integration tests for <see cref="AuthController"/>.
/// </summary>
[Trait(Layer.Application, Category.Services)]
[Collection(nameof(PostgresCollection))]
public class AuthControllerTests : IAsyncLifetime
{
    private readonly AuthTestWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly IServiceScope _scope;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthControllerTests"/> class.
    /// </summary>
    /// <param name="postgresFixture"></param>
    public AuthControllerTests(PostgresFixture postgresFixture)
    {
        _factory = new AuthTestWebApplicationFactory(postgresFixture, new MongoFixture());
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });
        _scope = _factory.Services.CreateScope();
    }

    /// <summary>
    /// Purpose: Verify Google sign-in returns an auth token pair.
    /// Should: Return 200 and the expected token payload.
    /// When: The request contains a valid idToken.
    /// </summary>
    [Fact]
    public async Task GoogleSignIn_Should_Return_Token()
    {
        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/google", new GoogleSignInRequest("id-token"));
        var token = await response.Content.ReadFromJsonAsync<AuthTokenDto>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(token);
        Assert.Equal(TestIdentityService.Token, token);
    }

    /// <summary>
    /// Purpose: Verify refresh endpoint returns a rotated token pair.
    /// Should: Return 200 and the expected token payload.
    /// When: A valid refresh token is provided.
    /// </summary>
    [Fact]
    public async Task Refresh_Should_Return_Token()
    {
        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/refresh", new RefreshTokenRequest("refresh-token"));
        var token = await response.Content.ReadFromJsonAsync<AuthTokenDto>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(token);
        Assert.Equal(TestIdentityService.Token, token);
    }

    /// <summary>
    /// Purpose: Verify one-time code exchange returns auth tokens.
    /// Should: Return 200 and the expected token payload.
    /// When: A valid one-time code is provided.
    /// </summary>
    [Fact]
    public async Task Exchange_Should_Return_Token()
    {
        // Arrange
        var store = _scope.ServiceProvider.GetRequiredService<IOneTimeCodeStore>();
        var code = store.Create(TestIdentityService.Token, TimeSpan.FromMinutes(5));

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/exchange", new AuthCodeExchangeRequest { Code = code });
        var token = await response.Content.ReadFromJsonAsync<AuthTokenDto>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(token);
        Assert.Equal(TestIdentityService.Token, token);
    }

    /// <summary>
    /// Purpose: Verify whoami returns user info for authenticated calls.
    /// Should: Return 200 and the expected identity payload.
    /// When: Request contains a valid access token.
    /// </summary>
    [Fact]
    public async Task Whoami_Should_Return_Identity()
    {
        // Arrange
        var tokenIssuer = _scope.ServiceProvider.GetRequiredService<IAppTokenIssuer>();
        var token = tokenIssuer.Issue(Guid.NewGuid(), "User");
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/auth/whoami");
        request.Headers.Authorization = new("Bearer", token);

        // Act
        var response = await _client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            throw new Xunit.Sdk.XunitException(
                $"Expected 200 OK but got {(int)response.StatusCode}. Body: {body}");
        }

        var me = await response.Content.ReadFromJsonAsync<IdentityMe>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(me);
        Assert.Equal(TestIdentityService.Me, me);
    }

    /// <summary>
    /// Purpose: Verify whoami requires authentication.
    /// Should: Return 401 without a token.
    /// When: Request is unauthenticated.
    /// </summary>
    [Fact]
    public async Task Whoami_Without_Token_Should_Return_401()
    {
        // Act
        var response = await _client.GetAsync("/api/auth/whoami");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    /// <summary>
    /// Purpose: Verify error contract for invalid refresh request.
    /// Should: Return a ProblemDetails payload with expected fields.
    /// When: refreshToken is missing.
    /// </summary>
    [Fact]
    public async Task Refresh_Without_Token_Should_Return_ProblemDetails()
    {
        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/refresh", new RefreshTokenRequest(""));
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(problem);
        Assert.Equal("Invalid request", problem!.Title);
        Assert.Equal("refreshToken is required.", problem.Detail);
        Assert.Equal((int)HttpStatusCode.BadRequest, problem.Status);
    }

    /// <inheritdoc />
    public Task InitializeAsync()
        => Task.CompletedTask;

    /// <inheritdoc />
    public async Task DisposeAsync()
    {
        _scope.Dispose();
        _client.Dispose();
        await _factory.DisposeAsync();
    }

    private sealed class AuthTestWebApplicationFactory(PostgresFixture postgresFixture, MongoFixture mongoFixture)
        : CvaWebApplicationFactory(postgresFixture, mongoFixture)
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureServices(services =>
            {
                services.RemoveAll<IIdentityService>();
                services.AddSingleton<IIdentityService, TestIdentityService>();

                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", _ => { });
            });
        }
    }

    private sealed class TestIdentityService : IIdentityService
    {
        public static readonly Guid UserId = Guid.Parse("4A4F4EC6-6C7D-4E8B-A97A-8B4FC0C1B22E");
        public static readonly AuthTokenDto Token = new("access-token", "refresh-token", UserId);
        public static readonly IdentityMe Me = new(UserId, "User", "user@example.com");

        public Task<AuthTokenDto> SignInWithGoogleAsync(string googleIdToken, CancellationToken ct)
            => Task.FromResult(Token);

        public Task<AuthTokenDto> RefreshAsync(string refreshToken, CancellationToken ct)
            => Task.FromResult(Token);

        public Task<IdentityMe> GetMeAsync(CancellationToken ct)
            => Task.FromResult(Me);
    }

    private sealed class TestAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
    {
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("Authorization", out var headerValue))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            if (!headerValue.ToString().StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid auth scheme."));
            }

            var claims = new[]
            {
                new Claim(CustomClaimTypes.Subject, TestIdentityService.UserId.ToString()),
                new Claim(CustomClaimTypes.Role, "User")
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Test");

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
