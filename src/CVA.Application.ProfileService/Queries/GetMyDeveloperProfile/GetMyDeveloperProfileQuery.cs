namespace CVA.Application.ProfileService;

/// <summary>
/// Query to retrieve the profile of the currently authenticated developer.
/// </summary>
public sealed record GetMyDeveloperProfileQuery : IQuery<DeveloperProfileDto>;