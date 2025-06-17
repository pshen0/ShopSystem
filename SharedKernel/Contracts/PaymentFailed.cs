using ProtoBuf;

namespace SharedKernel.Contracts;

[ProtoContract]
public class PaymentFailed
{
    [ProtoMember(1)] public string OrderId { get; set; } = null!;
    [ProtoMember(2)] public string UserId { get; set; } = null!;
    [ProtoMember(3)] public string EventId { get; set; } = null!;
    [ProtoMember(4)] public long OccurredAtTicks { get; set; }
    [ProtoMember(5)] public string Reason { get; set; } = null!;
}