using MediatR;
using Order.App.DTO;

namespace Order.App.Queries;

public record GetOrdersQuery(string UserId) : IRequest<IEnumerable<OrderDto>>;