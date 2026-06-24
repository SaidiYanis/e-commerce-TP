using Ecommerce.BackOffice.Catalog.Domain;

namespace Ecommerce.BackOffice.Catalog.Ports;

public interface IProductRepository
{
    Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Product?> GetByIdAsync(Guid productId, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Guid productId, CancellationToken cancellationToken = default);

    Task AddAsync(Product product, CancellationToken cancellationToken = default);

    Task UpdateAsync(Product product, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid productId, CancellationToken cancellationToken = default);
}
