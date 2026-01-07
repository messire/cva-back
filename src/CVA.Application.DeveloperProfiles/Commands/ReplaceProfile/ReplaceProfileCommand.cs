namespace CVA.Application.DeveloperProfiles;

/// <summary>
/// Command to replace the entire developer profile.
/// </summary>
/// <param name="Request">The request containing the new profile details.</param>
public sealed record ReplaceProfileCommand(ReplaceProfileRequest Request) : ICommand<DeveloperProfileDto>;