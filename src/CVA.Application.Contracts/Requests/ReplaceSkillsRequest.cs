namespace CVA.Application.Contracts;

/// <summary>
/// Represents a request to replace a developer's skills with new ones.
/// </summary>
public sealed record ReplaceSkillsRequest
{
    /// <summary>
    /// The new set of skills to replace the existing ones.
    /// </summary>
    public string[] Skills { get; init; } = [];
}