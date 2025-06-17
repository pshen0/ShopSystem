using Microsoft.EntityFrameworkCore;
using Payment.Domain;
using SharedKernel.Messaging;

namespace Payment.Infrastructure;

public class PaymentDbContext : DbContext, IOutboxAccessor
{
    public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options) { }

    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<InboxMessage> InboxMessages => Set<InboxMessage>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    public IQueryable<OutboxMessageBase> Outbox => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Account>().HasKey(x => x.Id);
        b.Entity<Account>().HasIndex(x => x.UserId).IsUnique();
        b.Entity<InboxMessage>().HasKey(x => x.Id);
        b.Entity<InboxMessage>().HasIndex(x => x.EventId).IsUnique();
        b.Entity<OutboxMessage>().HasKey(x => x.Id);
    }
}