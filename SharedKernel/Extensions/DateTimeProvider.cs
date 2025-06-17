namespace SharedKernel.Extensions;

public static class DateTimeProvider
{
    public static Func<DateTime> UtcNow { get; set; } = () => DateTime.UtcNow;
}