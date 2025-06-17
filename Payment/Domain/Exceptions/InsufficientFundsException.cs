namespace Payment.Domain.Exceptions;

public class InsufficientFundsException : Exception
{
    public InsufficientFundsException() : base("not_enough_money") { }
}