using MediatR;

namespace Payment.App.Commands;

public record TopAccount(string UserId, long Amount) : IRequest<long>;