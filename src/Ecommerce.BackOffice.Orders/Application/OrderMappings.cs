using Ecommerce.BackOffice.Orders.Domain;

namespace Ecommerce.BackOffice.Orders.Application;

internal static class OrderMappings
{
    public static OrderDto ToDto(this Order order) =>
        new(
            order.Id,
            order.CreatedAt,
            order.Status,
            order.Lines
                .Select(line => new OrderLineDto(
                    line.ProductId,
                    line.UnitPrice,
                    line.Quantity,
                    line.TotalPrice))
                .ToArray(),
            order.TotalPrice,
            order.PaidAt,
            order.CancelledAt);
}
