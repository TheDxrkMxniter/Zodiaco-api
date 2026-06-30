using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zodiaco.Api.DTOs;
using Zodiaco.Api.Services;

namespace Zodiaco.Api.Controllers;

[ApiController]
[Route("api/leads")]
public sealed class LeadsController(LeadsService leadsService) : ControllerBase
{
    /// <summary>
    /// Creates a public general contact lead from the website.
    /// </summary>
    /// <param name="request">Lead data submitted by the website visitor.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <returns>The created lead confirmation.</returns>
    [HttpPost]
    [EndpointSummary("Create a public website lead.")]
    [EndpointDescription("Stores a general public contact lead with status PENDING_REVIEW.")]
    [ProducesResponseType(typeof(CreateLeadResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateLeadResponseDto>> CreateLead(
        [FromBody] CreateLeadRequestDto request,
        CancellationToken cancellationToken)
    {
        var response = await leadsService.CreateLeadAsync(request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, response);
    }
}
