using Ecommerce.BackOffice.Orders.Domain;

namespace Ecommerce.BackOffice.Orders.Ports;

public interface IOrderRepository
{
    Task<IReadOnlyCollection<Order>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default);

    Task AddAsync(Order order, CancellationToken cancellationToken = default);

    Task UpdateAsync(Order order, CancellationToken cancellationToken = default);
}
