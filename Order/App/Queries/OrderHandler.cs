using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.App.DTO;
using Order.App.Queries;
using Order.Infrastructure;

namespace Order.App.Queries;

public class GetOrdersHandler : IRequestHandler<GetOrdersQuery, IEnumerable<OrderDto>>
{
    readonly OrderDbContext db;
    public GetOrdersHandler(OrderDbContext db) => this.db = db;

    public async Task<IEnumerable<OrderDto>> Handle(GetOrdersQuery q, CancellationToken ct)
    {
        return await db.Orders
            .Where(o => o.UserId == q.UserId)
            .Select(o => new OrderDto(o.Id, o.Amount, o.Description, o.Status.ToString()))
            .ToListAsync(ct);
    }
}