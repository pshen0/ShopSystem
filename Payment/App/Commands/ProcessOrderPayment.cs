using MediatR;

namespace Payment.App.Commands;

public record ProcessOrderPayment(string EventId, string UserId, string OrderId, long Amount, byte[] Raw) : IRequest<Unit>;