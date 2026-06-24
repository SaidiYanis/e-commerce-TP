using Ecommerce.BackOffice.Orders.Domain;

namespace Ecommerce.BackOffice.Orders.Application;

public sealed record OrderDto(
    Guid Id,
    DateTime CreatedAt,
    OrderStatus Status,
    IReadOnlyCollection<OrderLineDto> Lines,
    decimal TotalPrice,
    DateTime? PaidAt,
    DateTime? CancelledAt);
