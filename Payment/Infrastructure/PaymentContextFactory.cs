using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Payment.Infrastructure;

public class PaymentContextFactory : IDesignTimeDbContextFactory<PaymentDbContext>
{
    public PaymentDbContext CreateDbContext(string[] args)
    {
        var conn = Environment.GetEnvironmentVariable("PAYMENTS_DB_CONNECTION")
                   ?? "Host=localhost;Username=postgres;Password=postgres;Database=payments_dev";
        var builder = new DbContextOptionsBuilder<PaymentDbContext>();
        builder.UseNpgsql(conn);
        return new PaymentDbContext(builder.Options);
    }
}