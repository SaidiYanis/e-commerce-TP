using Ecommerce.BackOffice.Catalog.Domain;
using Ecommerce.BackOffice.Catalog.Ports;

namespace Ecommerce.BackOffice.Infrastructure.Repositories;

public sealed class InMemoryCategoryRepository : ICategoryRepository
{
    private readonly InMemoryRepository<Category> _repository = new();

    public Task<IReadOnlyCollection<Category>> GetAllAsync(CancellationToken cancellationToken = default) =>
        _repository.GetAllAsync();

    public Task<Category?> GetByIdAsync(Guid categoryId, CancellationToken cancellationToken = default) =>
        _repository.GetByIdAsync(categoryId);

    public Task<bool> ExistsAsync(Guid categoryId, CancellationToken cancellationToken = default) =>
        _repository.ExistsAsync(categoryId);

    public Task AddAsync(Category category, CancellationToken cancellationToken = default) =>
        _repository.SaveAsync(category);

    public Task UpdateAsync(Category category, CancellationToken cancellationToken = default) =>
        _repository.SaveAsync(category);

    public Task DeleteAsync(Guid categoryId, CancellationToken cancellationToken = default) =>
        _repository.DeleteAsync(categoryId);
}
