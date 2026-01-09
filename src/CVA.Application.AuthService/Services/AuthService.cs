using CVA.Application.Abstractions;
using CVA.Application.Contracts;
using CVA.Domain.Interfaces;
using CVA.Domain.Models;
using CVA.Infrastructure.Auth;

namespace CVA.Application.AuthService;

/// <summary>
/// Default authentication service implementation.
/// </summary>
internal sealed class AuthService(IUserRepository users, IAppTokenIssuer jwtTokenGenerator) : IAuthService
{
    /// <inheritdoc />
    public async Task<Result<AuthTokenDto>> SignInWithGoogleAsync(string googleIdToken, CancellationToken ct)
        => AppError.Failure("This method must be called via Web after Google token verification.");

    /// <summary>
    /// Creates or loads a user by email and issues app JWT.
    /// </summary>
    public async Task<Result<AuthTokenDto>> IssueTokenForGoogleUserAsync(string email, string firstName, string lastName, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return AppError.Failure("Email is missing.");
        }

        if (string.IsNullOrWhiteSpace(firstName))
        {
            firstName = "User";
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            lastName = "User";
        }

        var user = await users.GetByEmailAsync(email, ct);
        if (user is null)
        {
            user = User.Create(firstName, lastName, email);
            user = await users.CreateAsync(user, ct);
            if (user is null) return AppError.Failure("Failed to create user.");
        }

        var accessToken = jwtTokenGenerator.Issue(user.Id, role: "User");

        return new AuthTokenDto(accessToken, user.Id);
    }
}