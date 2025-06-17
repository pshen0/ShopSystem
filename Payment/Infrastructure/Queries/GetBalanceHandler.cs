using MediatR;
using Payment.App.DTO;
using Payment.App.Queries;

namespace Payment.Infrastructure.Queries;

public class BalanceHandler : IRequestHandler<BalanceQuery, AccountDTO>
{
    readonly PaymentDbContext db;
    public BalanceHandler(PaymentDbContext db) => this.db = db;
    public Task<AccountDTO> Handle(BalanceQuery request, CancellationToken ct)
    {
        var acc = db.Accounts.First(a => a.UserId == request.UserId);
        return Task.FromResult(new AccountDTO(acc.Id, acc.Balance));
    }
}