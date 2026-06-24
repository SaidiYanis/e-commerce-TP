using Ecommerce.BackOffice.Catalog.Domain;
using Ecommerce.BackOffice.Catalog.Ports;

namespace Ecommerce.BackOffice.Infrastructure.Repositories;

public sealed class InMemoryProductRepository : IProductRepository
{
    private readonly InMemoryRepository<Product> _repository = new();

    public Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken cancellationToken = default) =>
        _repository.GetAllAsync();

    public Task<Product?> GetByIdAsync(Guid productId, CancellationToken cancellationToken = default) =>
        _repository.GetByIdAsync(productId);

    public Task<bool> ExistsAsync(Guid productId, CancellationToken cancellationToken = default) =>
        _repository.ExistsAsync(productId);

    public Task AddAsync(Product product, CancellationToken cancellationToken = default) =>
        _repository.SaveAsync(product);

    public Task UpdateAsync(Product product, CancellationToken cancellationToken = default) =>
        _repository.SaveAsync(product);

    public Task DeleteAsync(Guid productId, CancellationToken cancellationToken = default) =>
        _repository.DeleteAsync(productId);
}
