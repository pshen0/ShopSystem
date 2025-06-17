using Microsoft.AspNetCore.Mvc;

namespace Order.Api.Controllers;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult OkStatus() => Ok("ok");
}