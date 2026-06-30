using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zodiaco.Api.DTOs;
using Zodiaco.Api.Services;

namespace Zodiaco.Api.Controllers;

[ApiController]
[Route("api/quote-requests")]
public sealed class QuoteRequestsController(QuoteRequestsService quoteRequestsService) : ControllerBase
{
    /// <summary>
    /// Creates a public quote request for a published truck.
    /// </summary>
    /// <param name="request">Quote request data submitted by the website visitor.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>The created quote request confirmation when the truck is public.</returns>
    [HttpPost]
    [EndpointSummary("Create a public quote request.")]
    [EndpointDescription("Stores a quote request linked to a public truck with status PENDING_REVIEW.")]
    [ProducesResponseType(typeof(CreateQuoteRequestResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CreateQuoteRequestResponseDto>> CreateQuoteRequest(
        [FromBody] CreateQuoteRequestDto request,
        CancellationToken cancellationToken)
    {
        var response = await quoteRequestsService.CreateQuoteRequestAsync(request, cancellationToken);
        if (response is null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Truck not found.",
                Detail = "The requested truck does not exist or is not publicly available.",
                Status = StatusCodes.Status404NotFound
            });
        }

        return StatusCode(StatusCodes.Status201Created, response);
    }
}
