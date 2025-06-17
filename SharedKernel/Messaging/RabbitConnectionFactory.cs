using RabbitMQ.Client;

namespace SharedKernel.Messaging;

public class RabbitConnectionFactory
{
    readonly IConnection connection;
    public RabbitConnectionFactory(string hostName, string userName, string password)
    {
        var factory = new ConnectionFactory { HostName = hostName, UserName = userName, Password = password, DispatchConsumersAsync = true };
        connection = factory.CreateConnection();
    }
    public IModel CreateChannel() => connection.CreateModel();
}