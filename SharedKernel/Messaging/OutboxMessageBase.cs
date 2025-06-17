namespace SharedKernel.Messaging;

public abstract class OutboxMessageBase
{
    public Guid Id { get; init; }
    public string Type { get; init; } = null!;
    public byte[] Payload { get; init; } = null!;
    public DateTime OccurredOn { get; init; }
    public DateTime? ProcessedOn { get; set; }
}