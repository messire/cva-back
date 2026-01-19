using CVA.Application.ProfileService;
using CVA.Domain.Interfaces;
using CVA.Presentation.Web.Dtos;

namespace CVA.Presentation.Web;

/// <summary>
/// Public read-only API for developer profiles.
/// </summary>
[ApiController]
[Route("api/catalog")]
public sealed class ProfilesCatalogController(QueryExecutor queries) : ControllerBase
{
    /// <summary>
    /// Returns profiles catalog (cards) with mandatory paging and sorting.
    /// </summary>
    /// <param name="request">Catalog query parameters.</param>
    /// <param name="ct">Cancellation token.</param>
    [HttpGet]
    [ProducesResponseType(typeof(CatalogResponseDto<ProfileCardDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetCatalog(
        GetCatalogRequest request,
        CancellationToken ct)
    {
        var query = new GetProfilesCatalogQuery(
            Search: request.Search,
            Skills: request.Skills ?? [],
            OpenToWork: request.OpenToWork,
            VerificationStatus: request.VerificationStatus,
            Page: request.Page ?? 1,
            PageSize: request.PageSize ?? 10,
            SortField: request.SortField ?? ProfilesSortFields.UpdatedAt,
            SortOrder: request.SortOrder ?? SortOrders.Desc);

        var result = await queries.ExecuteAsync<GetProfilesCatalogQuery, CatalogResponseDto<ProfileCardDto>>(query, ct);
        return this.ToActionResult(result);
    }

    /// <summary>
    /// Returns a single developer profile by id.
    /// </summary>
    /// <param name="id">Developer profile id.</param>
    /// <param name="ct">Cancellation token.</param>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetById(Guid id, CancellationToken ct)
    {
        var query = new GetProfileByIdQuery(id);
        var result = await queries.ExecuteAsync<GetProfileByIdQuery, ProfileDto>(query, ct);
        return this.ToActionResult(result);
    }
}