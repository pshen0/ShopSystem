namespace Order.Domain;

public class IOrder
{
    public Guid Id { get; init; }
    public string UserId { get; init; } = null!;
    public long Amount { get; init; }
    public string Description { get; init; } = null!;
    public OrderStatus Status { get; set; } = OrderStatus.New;
}