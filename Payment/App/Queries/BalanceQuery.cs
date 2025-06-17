using MediatR;
using Payment.App.DTO;

namespace Payment.App.Queries;

public record BalanceQuery(string UserId) : IRequest<AccountDTO>;