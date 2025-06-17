using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using SharedKernel.Messaging;

namespace SharedKernel.Extensions;

public static class ServiceCollection
{
    public static IServiceCollection AddRabbit(this IServiceCollection services, string host, string user, string pass)
    {
        var factory = new ConnectionFactory { HostName = host, UserName = user, Password = pass, DispatchConsumersAsync = true };
        var connection = factory.CreateConnection();
        services.AddSingleton(connection);
        services.AddSingleton<IMessagePublisher>(sp =>
            new RabbitPublisher(sp.GetRequiredService<IConnection>().CreateModel()));
        services.AddSingleton<IMessageConsumer>(sp =>
            new RabbitConsumer(sp.GetRequiredService<IConnection>().CreateModel()));
        return services;
    }
}