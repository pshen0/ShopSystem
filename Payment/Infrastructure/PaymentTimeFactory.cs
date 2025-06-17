using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Payment.Infrastructure;

public class PaymentTimeFactory : IDesignTimeDbContextFactory<PaymentDbContext>
{
    public PaymentDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<PaymentDbContext>()
            .UseNpgsql("Host=localhost;Username=postgres;Password=postgres;Database=payments")
            .Options;

        return new PaymentDbContext(options);
    }
}