using Microsoft.AspNetCore.Mvc;
using Vaede.Api.Common;
using Vaede.Api.DTOs;

namespace Vaede.Api.Controllers;

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
