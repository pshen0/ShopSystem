namespace Payment.Domain;

public class Account
{
    public Guid Id { get; init; }
    public string UserId { get; init; } = null!;
    public long Balance { get; private set; }

    public void Credit(long amount) => Balance += amount;
    public bool TryDebit(long amount)
    {
        if (Balance < amount) return false;
        Balance -= amount;
        return true;
    }
}