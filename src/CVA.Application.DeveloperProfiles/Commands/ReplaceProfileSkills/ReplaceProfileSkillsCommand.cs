namespace CVA.Application.DeveloperProfiles;

/// <summary>
/// Command to replace the skills in the developer profile.
/// </summary>
/// <param name="Skills">The new set of skills.</param>
public sealed record ReplaceProfileSkillsCommand(string[] Skills) : ICommand<DeveloperProfileDto>;