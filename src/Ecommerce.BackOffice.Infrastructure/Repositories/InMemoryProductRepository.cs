using Ecommerce.BackOffice.Catalog.Domain;
using Ecommerce.BackOffice.Catalog.Ports;

namespace Ecommerce.BackOffice.Infrastructure.Repositories;

public sealed class InMemoryProductRepository : IProductRepository
{
    private readonly Dictionary<Guid, Product> _products = new();

    public Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyCollection<Product>>(_products.Values.ToArray());

    public Task<Product?> GetByIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        _products.TryGetValue(productId, out var product);
        return Task.FromResult(product);
    }

    public Task<bool> ExistsAsync(Guid productId, CancellationToken cancellationToken = default) =>
        Task.FromResult(_products.ContainsKey(productId));

    public Task AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        _products[product.Id] = product;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        _products[product.Id] = product;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        _products.Remove(productId);
        return Task.CompletedTask;
    }
}
