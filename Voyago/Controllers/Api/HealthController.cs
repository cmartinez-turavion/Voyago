using Microsoft.AspNetCore.Mvc;

namespace Voyago.Controllers.Api;

[ApiController]
[Route("api/v1/health")]
public sealed class HealthController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<object> Get() => Ok(new { status = "ok" });
}
