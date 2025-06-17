using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace SharedKernel.Messaging;

public class OutboxWorker<TContext, TMessage> : BackgroundService
    where TContext : DbContext, IOutboxAccessor
    where TMessage : OutboxMessageBase
{
    readonly IServiceProvider sp;
    readonly IMessagePublisher publisher;
    readonly string exchange;
    public OutboxWorker(IServiceProvider sp, IMessagePublisher publisher, string exchange)
    {
        this.sp = sp;
        this.publisher = publisher;
        this.exchange = exchange;
    }
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TContext>();
            var msgs = await db.Outbox.OfType<TMessage>().Where(x => x.ProcessedOn == null).Take(20).ToListAsync(ct);
            foreach (var m in msgs)
            {
                await publisher.PublishAsync(exchange, m.Type, m.Payload, ct);
                m.ProcessedOn = DateTime.UtcNow;
            }
            await db.SaveChangesAsync(ct);
            await Task.Delay(500, ct);
        }
    }
}