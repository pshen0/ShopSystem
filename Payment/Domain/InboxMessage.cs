namespace Payment.Domain;

public class InboxMessage
{
    public Guid Id { get; init; }
    public string EventId { get; init; } = null!;
    public string Type { get; init; } = null!;
    public byte[] Payload { get; init; } = null!;
    public DateTime ReceivedOn { get; init; }
}