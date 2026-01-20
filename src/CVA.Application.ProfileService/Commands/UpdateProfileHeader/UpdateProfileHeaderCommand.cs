namespace CVA.Application.ProfileService;

/// <summary>
/// Command to update the header information (name, title, etc.) in the developer profile.
/// </summary>
/// <param name="Request">The request containing the new header details.</param>
public sealed record UpdateProfileHeaderCommand(UpdateProfileHeaderRequest Request) : ICommand<ProfileDto>;