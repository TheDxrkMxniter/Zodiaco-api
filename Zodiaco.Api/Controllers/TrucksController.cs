using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zodiaco.Api.DTOs;
using Zodiaco.Api.Services;

namespace Zodiaco.Api.Controllers;

[ApiController]
[Route("api/trucks")]
public sealed class TrucksController(TrucksService trucksService) : ControllerBase
{
    /// <summary>
    /// Lists the public catalog of published trucks for the inventory page.
    /// </summary>
    /// <param name="query">Public catalog filters, search terms, and pagination options.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>A paginated list of published trucks.</returns>
    [HttpGet]
    [EndpointSummary("List public published trucks.")]
    [EndpointDescription("Returns the public truck catalog with filters by type, brand, year, location, price, text search, and pagination.")]
    [ProducesResponseType(typeof(TruckListResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<TruckListResponseDto>> GetPublicCatalog(
        [FromQuery] TruckListQueryDto query,
        CancellationToken cancellationToken)
    {
        var response = await trucksService.GetPublicCatalogAsync(query, cancellationToken);
        return Ok(response);
    }
}
