using MediatR;

namespace Order.App.Commands;

public record CreateOrder(string UserId, long Amount, string Description) : IRequest<Guid>;