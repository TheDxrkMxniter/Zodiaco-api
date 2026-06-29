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

    /// <summary>
    /// Gets the public detail of a published truck using its slug.
    /// </summary>
    /// <param name="slug">Public truck slug to resolve.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>The public detail for the requested truck, when available.</returns>
    [HttpGet("{slug}")]
    [EndpointSummary("Get public truck detail by slug.")]
    [EndpointDescription("Returns the public truck detail with technical data, commercial data, images, location, and related trucks.")]
    [ProducesResponseType(typeof(TruckDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TruckDetailDto>> GetPublicTruckDetail(
        [FromRoute] string slug,
        CancellationToken cancellationToken)
    {
        var response = await trucksService.GetPublicTruckDetailAsync(slug, cancellationToken);
        if (response is null)
        {
            return NotFound();
        }

        return Ok(response);
    }
}
