using MediatR;
using Payment.Domain;
using Payment.Domain.Exceptions;
using Payment.App.Commands;
using ProtoBuf;
using SharedKernel.Contracts;
using SharedKernel.Extensions;

namespace Payment.Infrastructure.Commands;

public class CreateAccountHandler : IRequestHandler<CreateAccount, Guid>
{
    readonly PaymentDbContext db;
    public CreateAccountHandler(PaymentDbContext db) => this.db = db;

    public async Task<Guid> Handle(CreateAccount request, CancellationToken ct)
    {
        if (db.Accounts.Any(a => a.UserId == request.UserId))
            return db.Accounts.First(a => a.UserId == request.UserId).Id;
        var acc = new Account { Id = Guid.NewGuid(), UserId = request.UserId };
        db.Accounts.Add(acc);
        await db.SaveChangesAsync(ct);
        return acc.Id;
    }
}

public class TopAccountHandler : IRequestHandler<TopAccount, long>
{
    readonly PaymentDbContext db;
    public TopAccountHandler(PaymentDbContext db) => this.db = db;

    public async Task<long> Handle(TopAccount request, CancellationToken ct)
    {
        var acc = db.Accounts.First(a => a.UserId == request.UserId);
        acc.Credit(request.Amount);
        await db.SaveChangesAsync(ct);
        return acc.Balance;
    }
}

public class ProcessOrderPaymentHandler : IRequestHandler<ProcessOrderPayment, Unit>
{
    readonly PaymentDbContext db;
    public ProcessOrderPaymentHandler(PaymentDbContext db) => this.db = db;

    public async Task<Unit> Handle(ProcessOrderPayment cmd, CancellationToken ct)
    {
        if (db.InboxMessages.Any(m => m.EventId == cmd.EventId)) return Unit.Value;
        var inbox = new InboxMessage
        {
            Id = Guid.NewGuid(),
            EventId = cmd.EventId,
            Type = "OrderCreated",
            Payload = cmd.Raw,
            ReceivedOn = DateTimeProvider.UtcNow()
        };
        db.InboxMessages.Add(inbox);
        var account = db.Accounts.FirstOrDefault(a => a.UserId == cmd.UserId);
        PaymentSucceeded ok;
        PaymentFailed fail;
        if (account != null && account.TryDebit(cmd.Amount))
        {
            ok = new PaymentSucceeded
            {
                OrderId = cmd.OrderId,
                UserId = cmd.UserId,
                EventId = Guid.NewGuid().ToString(),
                OccurredAtTicks = DateTimeProvider.UtcNow().Ticks
            };
            using var ms = new MemoryStream();
            Serializer.Serialize(ms, ok);
            db.OutboxMessages.Add(new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = nameof(PaymentSucceeded),
                Payload = ms.ToArray(),
                OccurredOn = DateTimeProvider.UtcNow()
            });
        }
        else
        {
            fail = new PaymentFailed
            {
                OrderId = cmd.OrderId,
                UserId = cmd.UserId,
                EventId = Guid.NewGuid().ToString(),
                OccurredAtTicks = DateTimeProvider.UtcNow().Ticks,
                Reason = "insufficient"
            };
            using var ms = new MemoryStream();
            Serializer.Serialize(ms, fail);
            db.OutboxMessages.Add(new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = nameof(PaymentFailed),
                Payload = ms.ToArray(),
                OccurredOn = DateTimeProvider.UtcNow()
            });
        }
        await db.SaveChangesAsync(ct);
        return Unit.Value;
    }
}