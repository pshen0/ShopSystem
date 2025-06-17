using MediatR;

namespace Payment.App.Commands;

public record CreateAccount(string UserId) : IRequest<Guid>;