namespace SharedKernel.Messaging;

public interface IMessagePublisher
{
    Task PublishAsync(string exchange, string routingKey, byte[] body, CancellationToken cancellationToken = default);
}