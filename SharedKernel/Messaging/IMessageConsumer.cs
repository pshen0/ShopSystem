namespace SharedKernel.Messaging;

public interface IMessageConsumer : IAsyncDisposable
{
    void Subscribe(string queue, Func<ReadOnlyMemory<byte>, Task<bool>> handler);
}