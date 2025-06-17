using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Payment.Infrastructure;

public class DbHealthCheck : IHealthCheck
{
    readonly PaymentDbContext db;
    public DbHealthCheck(PaymentDbContext db) => this.db = db;
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext _, CancellationToken ct = default)
    {
        var ok = await db.Database.CanConnectAsync(ct);
        return ok ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy();
    }
}