using CVA.Application.ProfileService;
using CVA.Infrastructure.ResumePdf;

namespace CVA.Presentation.Web;

/// <summary>
/// Public API for downloading a developer profile as a PDF.
/// </summary>
[ApiController]
[Route("api/catalog")]
public sealed class ResumeController(QueryExecutor queries, ResumePdfService pdf) : ControllerBase
{
    /// <summary>
    /// Generates (or returns cached) resume PDF and returns it as a file.
    /// User never sees any storage URLs.
    /// </summary>
    /// <param name="id">Developer profile id.</param>
    /// <param name="download">If true - forces download; otherwise opens inline (new tab friendly).</param>
    /// <param name="ct">Cancellation token.</param>
    [HttpGet("{id:guid}/resume.pdf")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Download(Guid id, [FromQuery] bool download, CancellationToken ct)
    {
        var query = new GetDeveloperProfileByIdQuery(id);
        var result = await queries.ExecuteAsync<GetDeveloperProfileByIdQuery, DeveloperProfileDto>(query, ct);

        if (!result.IsSuccess)
        {
            return this.ToActionResult(result);
        }

        var (content, fileName) = await pdf.GetOrCreateAsync(result.Value!, ct);

        if (download)
        {
            return File(content, "application/pdf", fileName);
        }

        Response.Headers.ContentDisposition = $"inline; filename=\"{fileName}\"";
        return File(content, "application/pdf");
    }
}