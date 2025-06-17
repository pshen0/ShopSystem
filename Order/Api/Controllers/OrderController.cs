using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.App.Commands;
using Order.App.Queries;

namespace Order.Api.Controllers;

[ApiController]
[Route("orders")]
public class OrderController : ControllerBase
{
    readonly IMediator mediator;

    public OrderController(IMediator mediator) => this.mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create([FromHeader(Name = "user_id")] string userId, [FromBody] CreateOrderRequest req)
    {
        var id = await mediator.Send(new CreateOrder(userId, req.Amount, req.Description));
        return Accepted(new { orderId = id });
    }

    [HttpGet]
    public async Task<IActionResult> List([FromHeader(Name = "user_id")] string userId)
    {
        var list = await mediator.Send(new GetOrdersQuery(userId));
        return Ok(list);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Status(Guid id, [FromHeader(Name = "user_id")] string userId)
    {
        var status = await mediator.Send(new GetOrderStatusQuery(userId, id));
        return Ok(new { status });
    }

    public record CreateOrderRequest(long Amount, string Description);
}