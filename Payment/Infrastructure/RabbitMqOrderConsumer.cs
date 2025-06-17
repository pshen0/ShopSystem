using MediatR;
using ProtoBuf;
using Microsoft.Extensions.Hosting;
using SharedKernel.Contracts;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Messaging;
using Payment.App.Commands;

namespace Payment.Infrastructure;

public class RabbitMqOrderConsumer : BackgroundService
{
    readonly IMessageConsumer consumer;
    readonly IServiceProvider sp;

    public RabbitMqOrderConsumer(IMessageConsumer consumer, IServiceProvider sp)
    {
        this.consumer = consumer;
        this.sp = sp;
    }

    protected override Task ExecuteAsync(CancellationToken ct)
    {
        consumer.Subscribe("orders.events", async body =>
        {
            using var scope = sp.CreateScope();
            var m = Serializer.Deserialize<OrderCreated>(body.Span);
            var med = scope.ServiceProvider.GetRequiredService<IMediator>();
            await med.Send(new ProcessOrderPayment(m.EventId, m.UserId, m.OrderId, m.Amount, body.ToArray()), ct);
            return true;
        });
        return Task.CompletedTask;
    }
}