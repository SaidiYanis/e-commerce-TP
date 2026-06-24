namespace Ecommerce.BackOffice.Orders.Ports;

public sealed record OrderProductSnapshot(
    Guid Id,
    decimal CurrentUnitPrice,
    int Stock);
