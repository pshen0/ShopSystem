using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payment.App.Commands;
using Payment.App.Queries;

namespace Payment.Api.Controllers;

[ApiController]
[Route("accounts")]
public class AccountController : ControllerBase
{
    readonly IMediator med;
    public AccountController(IMediator med) => this.med = med;

    [HttpPost]
    public async Task<IActionResult> Create([FromHeader(Name = "user_id")] string userId)
    {
        var id = await med.Send(new CreateAccount(userId));
        return Created(string.Empty, new { accountId = id });
    }

    [HttpPost("{id:guid}/top-up")]
    public async Task<IActionResult> _TopUp(Guid id, [FromHeader(Name = "user_id")] string userId, [FromBody] TopUp req)
    {
        var bal = await med.Send(new TopAccount(userId, req.Amount));
        return Ok(new { balance = bal });
    }

    [HttpGet("{id:guid}/balance")]
    public async Task<IActionResult> Balance(Guid id, [FromHeader(Name = "user_id")] string userId)
    {
        var dto = await med.Send(new BalanceQuery(userId));
        return Ok(dto);
    }

    public record TopUp(long Amount);
}