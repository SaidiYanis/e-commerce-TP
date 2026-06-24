namespace Ecommerce.BackOffice.Orders.Application;

public sealed record OrderLineDto(
    Guid ProductId,
    decimal UnitPrice,
    int Quantity,
    decimal TotalPrice);
