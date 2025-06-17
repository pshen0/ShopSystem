using MediatR;

namespace Order.App.Queries;

public record GetOrderStatusQuery(string UserId, Guid OrderId) : IRequest<string>;