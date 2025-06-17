using Microsoft.EntityFrameworkCore;
using Order.Domain;
using SharedKernel.Messaging;

namespace Order.Infrastructure;

public class OrderDbContext : DbContext, IOutboxAccessor
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

    public DbSet<IOrder> Orders => Set<IOrder>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    public IQueryable<OutboxMessageBase> Outbox => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<IOrder>().HasKey(x => x.Id);
        b.Entity<OutboxMessage>().HasKey(x => x.Id);
    }
}