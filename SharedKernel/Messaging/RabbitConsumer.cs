using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace SharedKernel.Messaging
{
    public class RabbitConsumer : IMessageConsumer
    {
        readonly IModel channel;
        readonly AsyncEventingBasicConsumer consumer;
        public RabbitConsumer(IModel channel)
        {
            this.channel = channel;
            consumer = new AsyncEventingBasicConsumer(channel);
        }
        public void Subscribe(string queue, Func<ReadOnlyMemory<byte>, Task<bool>> handler)
        {
            channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false, arguments: null);

            consumer.Received += async (_, ea) =>
            {
                var ok = await handler(ea.Body);
                if (ok) channel.BasicAck(ea.DeliveryTag, false);
                else    channel.BasicNack(ea.DeliveryTag, false, true);
            };
            channel.BasicConsume(queue, autoAck: false, consumer);
        }
        public ValueTask DisposeAsync()
        {
            channel.Close();
            channel.Dispose();
            return ValueTask.CompletedTask;
        }
    }
}