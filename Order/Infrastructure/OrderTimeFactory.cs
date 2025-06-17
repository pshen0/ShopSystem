using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Order.Infrastructure;

public class OrderTimeFactory : IDesignTimeDbContextFactory<OrderDbContext>
{
    public OrderDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<OrderDbContext>()
            .UseNpgsql("Host=localhost;Username=postgres;Password=postgres;Database=orders")
            .Options;

        return new OrderDbContext(options);
    }
}