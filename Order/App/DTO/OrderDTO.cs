namespace Order.App.DTO;

public record OrderDto(Guid Id, long Amount, string Description, string Status);