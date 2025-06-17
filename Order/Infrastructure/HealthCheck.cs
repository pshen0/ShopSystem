using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Order.Infrastructure;

public class DbHealthCheck : IHealthCheck
{
    readonly OrderDbContext db;

    public DbHealthCheck(OrderDbContext db) => this.db = db;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var ok = await db.Database.CanConnectAsync(cancellationToken);
        return ok ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy();
    }
}