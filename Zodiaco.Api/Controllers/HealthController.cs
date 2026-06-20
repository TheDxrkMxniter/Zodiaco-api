using Microsoft.AspNetCore.Mvc;
using Zodiaco.Api.Common;
using Zodiaco.Api.DTOs;

namespace Zodiaco.Api.Controllers;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(HealthResponseDto), StatusCodes.Status200OK)]
    public ActionResult<HealthResponseDto> Get()
    {
        return Ok(new HealthResponseDto("ok", ApplicationConstants.ServiceName));
    }
}
