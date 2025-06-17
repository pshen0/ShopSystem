using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedKernel.Extensions;
using SharedKernel.Messaging;
using Payment.App.Commands;
using Payment.App.Queries;
using Payment.Domain;
using Payment.Infrastructure;

var b = WebApplication.CreateBuilder(args);

b.Services.AddDbContext<PaymentDbContext>(o => 
    o.UseNpgsql(b.Configuration.GetConnectionString("Default")));

b.Services.AddMediatR(
    typeof(CreateAccount).Assembly,
    typeof(Payment.Infrastructure.Commands.CreateAccountHandler).Assembly
);

b.Services.AddRabbit(
    b.Configuration["Rabbit:Host"],
    b.Configuration["Rabbit:User"],
    b.Configuration["Rabbit:Pass"]);

b.Services.AddHostedService<RabbitMqOrderConsumer>();

b.Services.AddSingleton<IHostedService>(sp =>
    new OutboxWorker<PaymentDbContext, OutboxMessage>(
        sp,
        sp.GetRequiredService<IMessagePublisher>(),
        "payments.events"));

b.Services.AddHealthChecks().AddCheck<DbHealthCheck>("db");
b.Services.AddControllers();
b.Services.AddEndpointsApiExplorer();
b.Services.AddSwaggerGen();

var app = b.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();
    db.Database.EnsureCreated();
}
app.MapControllers();
app.MapHealthChecks("/healthz");

app.UseSwagger();
app.UseSwaggerUI();

app.Run();