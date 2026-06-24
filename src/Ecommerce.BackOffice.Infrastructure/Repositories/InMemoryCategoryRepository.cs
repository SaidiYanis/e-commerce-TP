using Ecommerce.BackOffice.Catalog.Domain;
using Ecommerce.BackOffice.Catalog.Ports;

namespace Ecommerce.BackOffice.Infrastructure.Repositories;

public sealed class InMemoryCategoryRepository : ICategoryRepository
{
    private readonly Dictionary<Guid, Category> _categories = new();

    public Task<IReadOnlyCollection<Category>> GetAllAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyCollection<Category>>(_categories.Values.ToArray());

    public Task<Category?> GetByIdAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        _categories.TryGetValue(categoryId, out var category);
        return Task.FromResult(category);
    }

    public Task<bool> ExistsAsync(Guid categoryId, CancellationToken cancellationToken = default) =>
        Task.FromResult(_categories.ContainsKey(categoryId));

    public Task AddAsync(Category category, CancellationToken cancellationToken = default)
    {
        _categories[category.Id] = category;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Category category, CancellationToken cancellationToken = default)
    {
        _categories[category.Id] = category;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        _categories.Remove(categoryId);
        return Task.CompletedTask;
    }
}
