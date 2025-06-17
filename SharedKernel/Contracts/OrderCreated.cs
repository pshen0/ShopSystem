using ProtoBuf;

namespace SharedKernel.Contracts;

[ProtoContract]
public class OrderCreated
{
    [ProtoMember(1)] public string OrderId { get; set; } = null!;
    [ProtoMember(2)] public string UserId { get; set; } = null!;
    [ProtoMember(3)] public long Amount { get; set; }
    [ProtoMember(4)] public string Description { get; set; } = null!;
    [ProtoMember(5)] public string EventId { get; set; } = null!;
    [ProtoMember(6)] public long OccurredAtTicks { get; set; }
}