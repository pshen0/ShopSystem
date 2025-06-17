using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Order.Domain;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf;
using SharedKernel.Contracts;
using SharedKernel.Messaging;

namespace Order.Infrastructure;

public class RabbitMqPaymentConsumer : BackgroundService
{
    readonly IMessageConsumer consumer;
    readonly IServiceProvider sp;

    public RabbitMqPaymentConsumer(IMessageConsumer consumer, IServiceProvider sp)
    {
        this.consumer = consumer;
        this.sp = sp;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        consumer.Subscribe("payments.events", async body =>
        {
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
            if (TryApply<PaymentSucceeded>(body, db, OrderStatus.Finished)) return true;
            if (TryApply<PaymentFailed>(body, db, OrderStatus.Cancelled)) return true;
            return false;
        });
        return Task.CompletedTask;
    }

    bool TryApply<T>(ReadOnlyMemory<byte> body, OrderDbContext db, OrderStatus status) where T : class
    {
        try
        {
            using var ms = new MemoryStream(body.ToArray());
            var msg = Serializer.Deserialize<T>(ms);
            var orderId = typeof(T).GetProperty("OrderId")!.GetValue(msg)!.ToString();
            var order = db.Orders.FirstOrDefault(o => o.Id == Guid.Parse(orderId!));
            if (order == null) return true;
            order.Status = status;
            db.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }
}