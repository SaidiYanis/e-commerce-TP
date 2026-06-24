using Ecommerce.BackOffice.Orders.Domain;
using Ecommerce.BackOffice.Orders.Ports;

namespace Ecommerce.BackOffice.Infrastructure.Repositories;

public sealed class InMemoryOrderRepository : IOrderRepository
{
    private readonly Dictionary<Guid, Order> _orders = new();

    public Task<IReadOnlyCollection<Order>> GetAllAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyCollection<Order>>(_orders.Values.ToArray());

    public Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        _orders.TryGetValue(orderId, out var order);
        return Task.FromResult(order);
    }

    public Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        _orders[order.Id] = order;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
    {
        _orders[order.Id] = order;
        return Task.CompletedTask;
    }
}
