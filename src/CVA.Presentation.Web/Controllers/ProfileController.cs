using Microsoft.AspNetCore.Authorization;

namespace CVA.Presentation.Web;

/// <summary>
/// Owner-only API for editing "my" developer profile.
/// </summary>
/// <param name="queries">Query executor.</param>
/// <param name="commands">Command executor.</param>
[ApiController]
[Authorize]
[Route("api/profile")]
public sealed class DeveloperProfilesController(QueryExecutor queries, CommandExecutor commands) : ControllerBase
{
    /// <summary>
    /// Returns current user's developer profile.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    [HttpGet]
    [ProducesResponseType(typeof(DeveloperProfileDto), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetMyProfile(CancellationToken ct)
    {
        var query = new GetMyDeveloperProfileQuery();
        var result = await queries.ExecuteAsync<GetMyDeveloperProfileQuery, DeveloperProfileDto>(query, ct);
        return this.ToActionResult(result);
    }

    /// <summary>
    /// Replaces the whole developer profile (full-save).
    /// </summary>
    /// <param name="request">New profile snapshot.</param>
    /// <param name="ct">Cancellation token.</param>
    [HttpPut]
    [ProducesResponseType(typeof(DeveloperProfileDto), StatusCodes.Status200OK)]
    public async Task<ActionResult> ReplaceMyProfile([FromBody] ReplaceProfileRequest request, CancellationToken ct)
    {
        var command = new ReplaceProfileCommand(request);
        var result = await commands.ExecuteAsync<ReplaceProfileCommand, DeveloperProfileDto>(command, ct);
        return this.ToActionResult(result);
    }

    /// <summary>
    /// Updates profile header section.
    /// </summary>
    /// <param name="request">Header fields.</param>
    /// <param name="ct">Cancellation token.</param>
    [HttpPatch("header")]
    [ProducesResponseType(typeof(DeveloperProfileDto), StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdateHeader([FromBody] UpdateProfileHeaderRequest request, CancellationToken ct)
    {
        var command = new UpdateProfileHeaderCommand(request);
        var result = await commands.ExecuteAsync<UpdateProfileHeaderCommand, DeveloperProfileDto>(command, ct);
        return this.ToActionResult(result);
    }

    /// <summary>
    /// Updates profile summary section.
    /// </summary>
    /// <param name="request">Summary fields.</param>
    /// <param name="ct">Cancellation token.</param>
    [HttpPatch("summary")]
    [ProducesResponseType(typeof(DeveloperProfileDto), StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdateSummary([FromBody] UpdateProfileSummaryRequest request, CancellationToken ct)
    {
        var command = new UpdateProfileSummaryCommand(request);
        var result = await commands.ExecuteAsync<UpdateProfileSummaryCommand, DeveloperProfileDto>(command, ct);
        return this.ToActionResult(result);
    }

    /// <summary>
    /// Updates profile contacts and social links.
    /// </summary>
    /// <param name="request">Contacts fields.</param>
    /// <param name="ct">Cancellation token.</param>
    [HttpPatch("contacts")]
    [ProducesResponseType(typeof(DeveloperProfileDto), StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdateContacts([FromBody] UpdateProfileContactsRequest request, CancellationToken ct)
    {
        var command = new UpdateProfileContactsCommand(request);
        var result = await commands.ExecuteAsync<UpdateProfileContactsCommand, DeveloperProfileDto>(command, ct);
        return this.ToActionResult(result);
    }

    /// <summary>
    /// Replaces skills list.
    /// </summary>
    /// <param name="request">Skills.</param>
    /// <param name="ct">Cancellation token.</param>
    [HttpPut("skills")]
    [ProducesResponseType(typeof(DeveloperProfileDto), StatusCodes.Status200OK)]
    public async Task<ActionResult> ReplaceSkills([FromBody] ReplaceSkillsRequest request, CancellationToken ct)
    {
        var command = new ReplaceProfileSkillsCommand(request.Skills);
        var result = await commands.ExecuteAsync<ReplaceProfileSkillsCommand, DeveloperProfileDto>(command, ct);
        return this.ToActionResult(result);
    }

    /// <summary>
    /// Creates a new work experience item.
    /// </summary>
    /// <param name="request">Work experience fields.</param>
    /// <param name="ct">Cancellation token.</param>
    [HttpPost("work-experiences")]
    [ProducesResponseType(typeof(DeveloperProfileDto), StatusCodes.Status200OK)]
    public async Task<ActionResult> CreateWorkExperience([FromBody] UpsertWorkExperienceRequest request, CancellationToken ct)
    {
        var command = new CreateWorkExperienceCommand(request);
        var result = await commands.ExecuteAsync<CreateWorkExperienceCommand, DeveloperProfileDto>(command, ct);
        return this.ToActionResult(result);
    }

    /// <summary>
    /// Updates an existing work experience item by id.
    /// </summary>
    /// <param name="workExperienceId">Work experience id.</param>
    /// <param name="request">Work experience fields.</param>
    /// <param name="ct">Cancellation token.</param>
    [HttpPut("work-experiences/{workExperienceId:guid}")]
    [ProducesResponseType(typeof(DeveloperProfileDto), StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdateWorkExperience(Guid workExperienceId, [FromBody] UpsertWorkExperienceRequest request, CancellationToken ct)
    {
        var command = new UpdateWorkExperienceCommand(workExperienceId, request);
        var result = await commands.ExecuteAsync<UpdateWorkExperienceCommand, DeveloperProfileDto>(command, ct);
        return this.ToActionResult(result);
    }

    /// <summary>
    /// Deletes a work experience item by id.
    /// </summary>
    /// <param name="workExperienceId">Work experience id.</param>
    /// <param name="ct">Cancellation token.</param>
    [HttpDelete("work-experiences/{workExperienceId:guid}")]
    [ProducesResponseType(typeof(DeveloperProfileDto), StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteWorkExperience(Guid workExperienceId, CancellationToken ct)
    {
        var command = new DeleteWorkExperienceCommand(workExperienceId);
        var result = await commands.ExecuteAsync<DeleteWorkExperienceCommand, DeveloperProfileDto>(command, ct);
        return this.ToActionResult(result);
    }

    /// <summary>
    /// Creates a new project.
    /// </summary>
    /// <param name="request">Project fields.</param>
    /// <param name="ct">Cancellation token.</param>
    [HttpPost("projects")]
    [ProducesResponseType(typeof(DeveloperProfileDto), StatusCodes.Status200OK)]
    public async Task<ActionResult> CreateProject([FromBody] UpsertProjectRequest request, CancellationToken ct)
    {
        var command = new CreateProjectCommand(request);
        var result = await commands.ExecuteAsync<CreateProjectCommand, DeveloperProfileDto>(command, ct);
        return this.ToActionResult(result);
    }

    /// <summary>
    /// Updates an existing project by id.
    /// </summary>
    /// <param name="projectId">Project id.</param>
    /// <param name="request">Project fields.</param>
    /// <param name="ct">Cancellation token.</param>
    [HttpPut("projects/{projectId:guid}")]
    [ProducesResponseType(typeof(DeveloperProfileDto), StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdateProject(Guid projectId, [FromBody] UpsertProjectRequest request, CancellationToken ct)
    {
        var command = new UpdateProjectCommand(projectId, request);
        var result = await commands.ExecuteAsync<UpdateProjectCommand, DeveloperProfileDto>(command, ct);
        return this.ToActionResult(result);
    }

    /// <summary>
    /// Deletes a project by id.
    /// </summary>
    /// <param name="projectId">Project id.</param>
    /// <param name="ct">Cancellation token.</param>
    [HttpDelete("projects/{projectId:guid}")]
    [ProducesResponseType(typeof(DeveloperProfileDto), StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteProject(Guid projectId, CancellationToken ct)
    {
        var command = new DeleteProjectCommand(projectId);
        var result = await commands.ExecuteAsync<DeleteProjectCommand, DeveloperProfileDto>(command, ct);
        return this.ToActionResult(result);
    }
}