namespace SharedKernel.DataModels;

public interface IEvent
{
    Guid Id { get; }
    DateTime OccurredAt { get; }
}