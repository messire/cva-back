namespace CVA.Application.ProfileService;

/// <summary>
/// Command to update the summary/about section in the developer profile.
/// </summary>
/// <param name="Request">The request containing the new summary text.</param>
public sealed record UpdateProfileSummaryCommand(UpdateProfileSummaryRequest Request) : ICommand<DeveloperProfileDto>;