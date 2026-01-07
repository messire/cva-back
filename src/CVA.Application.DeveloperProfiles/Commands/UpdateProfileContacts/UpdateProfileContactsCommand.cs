namespace CVA.Application.DeveloperProfiles;

/// <summary>
/// Command to update the contact information in the developer profile.
/// </summary>
/// <param name="Request">The request containing the new contact details.</param>
public sealed record UpdateProfileContactsCommand(UpdateProfileContactsRequest Request) : ICommand<DeveloperProfileDto>;