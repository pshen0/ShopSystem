using Microsoft.EntityFrameworkCore;

namespace SharedKernel.Messaging;

public interface IOutboxAccessor
{
    IQueryable<OutboxMessageBase> Outbox { get; }
    Task<int> SaveChangesAsync(CancellationToken ct);
}