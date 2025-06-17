using Microsoft.AspNetCore.Mvc;

namespace Payment.Api.Controllers;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult OkStatus() => Ok("ok");
}