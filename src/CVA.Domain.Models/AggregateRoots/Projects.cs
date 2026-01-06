namespace CVA.Domain.Models;

public sealed partial class DeveloperProfile
{
    /// <summary>
    /// Add a new project to the profile.
    /// </summary>
    /// <param name="name">The name of the project.</param>
    /// <param name="description">A brief description of the project.</param>
    /// <param name="icon">The icon associated with the project.</param>
    /// <param name="link">The link to the project.</param>
    /// <param name="techStack">The technology stack used in the project.</param>
    /// <param name="now">The current timestamp.</param>
    /// <returns>The ID of the newly added project.</returns>
    public ProjectId AddProject(
        ProjectName name,
        ProjectDescription? description,
        ProjectIcon? icon,
        ProjectLink link,
        IEnumerable<TechTag> techStack,
        DateTimeOffset now)
    {
        var projectId = new ProjectId(Guid.NewGuid());
        var project = ProjectItem.Create(projectId, name, description, icon, link, techStack);
        _projects.Add(project);
        Touch(now);
        return projectId;
    }

    /// <summary>
    /// Update an existing project.
    /// </summary>
    /// <param name="projectId">The ID of the project to update.</param>
    /// <param name="name">The new name for the project.</param>
    /// <param name="description">The new description for the project.</param>
    /// <param name="icon">The new icon for the project.</param>
    /// <param name="link">The new link for the project.</param>
    /// <param name="techStack">The new technology stack for the project.</param>
    /// <param name="now">The current timestamp.</param>
    public void UpdateProject(
        ProjectId projectId,
        ProjectName name,
        ProjectDescription? description,
        ProjectIcon? icon,
        ProjectLink link,
        IEnumerable<TechTag> techStack,
        DateTimeOffset now)
    {
        var project = FindProject(projectId);
        project.Update(name, description, icon, link, techStack);
        Touch(now);
    }

    /// <summary>
    /// Remove a project from the profile.
    /// </summary>
    /// <param name="projectId">The ID of the project to remove.</param>
    /// <param name="now">The current timestamp.</param>
    public void RemoveProject(ProjectId projectId, DateTimeOffset now)
    {
        Ensure.NotEmpty(projectId.Value, nameof(projectId));
        _projects.RemoveAll(x => x.Id.Equals(projectId));
        Touch(now);
    }

    private ProjectItem FindProject(ProjectId id)
    {
        Ensure.NotEmpty(id.Value, nameof(id));
        var project = _projects.Find(x => x.Id.Equals(id));
        if (project is null) throw new InvalidOperationException("Project not found.");
        return project;
    }
}