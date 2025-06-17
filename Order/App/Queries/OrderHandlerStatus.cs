using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.App.Queries;
using Order.Infrastructure;

namespace Order.App.Queries;

public class GetOrderStatusHandler : IRequestHandler<GetOrderStatusQuery, string>
{
    readonly OrderDbContext db;
    public GetOrderStatusHandler(OrderDbContext db) => this.db = db;

    public async Task<string> Handle(GetOrderStatusQuery q, CancellationToken ct)
    {
        var order = await db.Orders
            .Where(o => o.Id == q.OrderId && o.UserId == q.UserId)
            .Select(o => o.Status)
            .FirstAsync(ct);

        return order.ToString();
    }
}