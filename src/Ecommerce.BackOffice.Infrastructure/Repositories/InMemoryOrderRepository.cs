using Ecommerce.BackOffice.Orders.Domain;
using Ecommerce.BackOffice.Orders.Ports;

namespace Ecommerce.BackOffice.Infrastructure.Repositories;

public sealed class InMemoryOrderRepository : IOrderRepository
{
    private readonly InMemoryRepository<Order> _repository = new();

    public Task<IReadOnlyCollection<Order>> GetAllAsync(CancellationToken cancellationToken = default) =>
        _repository.GetAllAsync();

    public Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default) =>
        _repository.GetByIdAsync(orderId);

    public Task AddAsync(Order order, CancellationToken cancellationToken = default) =>
        _repository.SaveAsync(order);

    public Task UpdateAsync(Order order, CancellationToken cancellationToken = default) =>
        _repository.SaveAsync(order);
}
