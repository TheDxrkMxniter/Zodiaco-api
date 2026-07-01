using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zodiaco.Api.DTOs;
using Zodiaco.Api.Services;

namespace Zodiaco.Api.Controllers;

[ApiController]
[Route("api/sell-requests")]
public sealed class SellRequestsController(SellRequestsService sellRequestsService) : ControllerBase
{
    /// <summary>
    /// Creates a public sell request from the website.
    /// </summary>
    /// <param name="request">Sell request data submitted by the website visitor.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>The created sell request confirmation.</returns>
    [HttpPost]
    [EndpointSummary("Create a public sell request.")]
    [EndpointDescription("Stores a public sell request with status PENDING_REVIEW.")]
    [ProducesResponseType(typeof(CreateSellRequestResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateSellRequestResponseDto>> CreateSellRequest(
        [FromBody] CreateSellRequestDto request,
        CancellationToken cancellationToken)
    {
        var response = await sellRequestsService.CreateSellRequestAsync(request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, response);
    }
}
