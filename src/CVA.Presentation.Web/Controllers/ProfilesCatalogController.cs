namespace CVA.Presentation.Web;

/// <summary>
/// Public read-only API for developer profiles.
/// </summary>
[ApiController]
[Route("api/catalog")]
public sealed class ProfilesCatalogController(QueryExecutor queries) : ControllerBase
{
    /// <summary>
    /// Returns developer profiles catalog (cards).
    /// </summary>
    /// <param name="search">Free-text search.</param>
    /// <param name="skills">Skills filter.</param>
    /// <param name="openToWork">Open-to-work filter.</param>
    /// <param name="verificationStatus">Verification status filter.</param>
    /// <param name="ct">Cancellation token.</param>
    [HttpGet]
    [ProducesResponseType(typeof(DeveloperProfileCardDto[]), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetCatalog(
        [FromQuery] string? search,
        [FromQuery] string[]? skills,
        [FromQuery] bool? openToWork,
        [FromQuery] string? verificationStatus,
        CancellationToken ct)
    {
        var query = new GetDeveloperProfilesCatalogQuery(search, skills ?? [], openToWork, verificationStatus);
        var result = await queries.ExecuteAsync<GetDeveloperProfilesCatalogQuery, DeveloperProfileCardDto[]>(query, ct);
        return this.ToActionResult(result);
    }

    /// <summary>
    /// Returns a single developer profile by id.
    /// </summary>
    /// <param name="id">Developer profile id.</param>
    /// <param name="ct">Cancellation token.</param>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(DeveloperProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetById(Guid id, CancellationToken ct)
    {
        var query = new GetDeveloperProfileByIdQuery(id);
        var result = await queries.ExecuteAsync<GetDeveloperProfileByIdQuery, DeveloperProfileDto>(query, ct);
        return this.ToActionResult(result);
    }
}