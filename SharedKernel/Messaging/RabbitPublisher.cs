using RabbitMQ.Client;

namespace SharedKernel.Messaging;

public class RabbitPublisher : IMessagePublisher
{
    readonly IModel channel;
    public RabbitPublisher(IModel channel) => this.channel = channel;

    public Task PublishAsync(string exchange, string queue, byte[] body, CancellationToken cancellationToken = default)
    {
        channel.QueueDeclare(exchange, durable: true, exclusive: false, autoDelete: false, arguments: null);

        channel.BasicPublish(exchange: "",
                             routingKey: exchange,
                             mandatory: true,
                             basicProperties: null,
                             body: body);
        return Task.CompletedTask;
    }
}