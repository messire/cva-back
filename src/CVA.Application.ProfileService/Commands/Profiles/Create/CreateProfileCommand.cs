namespace CVA.Application.ProfileService;

/// <summary>
/// Command to create a developer profile for the current authenticated user.
/// </summary>
/// <param name="Request">The create profile request.</param>
public sealed record CreateProfileCommand(CreateProfileRequest Request) : ICommand<ProfileDto>;