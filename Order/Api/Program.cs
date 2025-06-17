using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Order.App.Commands;
using Order.App.Queries;
using Order.Domain;
using Order.Infrastructure;
using SharedKernel.Extensions;
using SharedKernel.Messaging;

var builder = WebApplication.CreateBuilder(args);

var conn = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<OrderDbContext>(o => o.UseNpgsql(conn));

builder.Services.AddMediatR(typeof(CreateOrder));

builder.Services.AddRabbit(
    builder.Configuration["Rabbit:Host"],
    builder.Configuration["Rabbit:User"],
    builder.Configuration["Rabbit:Pass"]);

builder.Services.AddSingleton<IHostedService>(sp =>
    new OutboxWorker<OrderDbContext, OutboxMessage>(
        sp,
        sp.GetRequiredService<IMessagePublisher>(),
        "orders.events"));

builder.Services.AddHostedService<RabbitMqPaymentConsumer>();

builder.Services.AddHealthChecks().AddCheck<DbHealthCheck>("db");
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
    db.Database.EnsureCreated();
}

app.MapControllers();
app.MapHealthChecks("/healthz");

app.UseSwagger();
app.UseSwaggerUI();

app.Run();