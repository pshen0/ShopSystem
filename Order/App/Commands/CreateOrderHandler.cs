using MediatR;
using Order.Domain;
using Order.Infrastructure;
using ProtoBuf;
using SharedKernel.Extensions;
using SharedKernel.Contracts;

namespace Order.App.Commands;

public class CreateOrderHandler : IRequestHandler<CreateOrder, Guid>
{
    readonly OrderDbContext db;

    public CreateOrderHandler(OrderDbContext db) => this.db = db;

    public async Task<Guid> Handle(CreateOrder request, CancellationToken ct)
    {
        var order = new IOrder { Id = Guid.NewGuid(), UserId = request.UserId, Amount = request.Amount, Description = request.Description };
        var evt = new OrderCreated
        {
            OrderId = order.Id.ToString(),
            UserId = order.UserId,
            Amount = order.Amount,
            Description = order.Description,
            EventId = Guid.NewGuid().ToString(),
            OccurredAtTicks = DateTimeProvider.UtcNow().Ticks
        };
        using var ms = new MemoryStream();
        Serializer.Serialize(ms, evt);
        var message = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            Type = nameof(OrderCreated),
            Payload = ms.ToArray(),
            OccurredOn = DateTimeProvider.UtcNow()
        };
        await db.Orders.AddAsync(order, ct);
        await db.OutboxMessages.AddAsync(message, ct);
        await db.SaveChangesAsync(ct);
        return order.Id;
    }
}