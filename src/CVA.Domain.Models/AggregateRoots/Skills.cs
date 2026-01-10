namespace CVA.Domain.Models;

public sealed partial class DeveloperProfile
{
    /// <summary>
    /// Add a new skill to the profile.
    /// </summary>
    /// <param name="skill">The skill to add.</param>
    /// <param name="now">The current timestamp.</param>
    public void AddSkill(SkillTag skill, DateTimeOffset now)
    {
        Ensure.NotNull(skill, nameof(skill));
        if (_skills.Any(tag => tag.Equals(skill))) return;

        _skills.Add(skill);
        Touch(now);
    }

    /// <summary>
    /// Remove a skill from the profile.
    /// </summary>
    /// <param name="skill">The skill to remove.</param>
    /// <param name="now">The current timestamp.</param>
    public void RemoveSkill(SkillTag skill, DateTimeOffset now)
    {
        Ensure.NotNull(skill, nameof(skill));
        _skills.RemoveAll(tag => tag.Equals(skill));
        Touch(now);
    }

    /// <summary>
    /// Replace all skills in the profile with the specified collection.
    /// </summary>
    /// <param name="skills">The collection of skills to replace with.</param>
    /// <param name="now">The current timestamp.</param>
    public void ReplaceSkills(IEnumerable<SkillTag> skills, DateTimeOffset now)
    {
        Ensure.NotNull(skills, nameof(skills));

        var normalized = skills
            .Where(tag => tag is not null)
            .Distinct()
            .ToArray();

        _skills.Clear();
        _skills.AddRange(normalized);

        Touch(now);
    }
}