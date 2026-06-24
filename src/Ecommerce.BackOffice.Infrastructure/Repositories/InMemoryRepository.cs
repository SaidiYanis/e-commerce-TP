using Ecommerce.BackOffice.SharedKernel.Abstractions;

namespace Ecommerce.BackOffice.Infrastructure.Repositories;

internal class InMemoryRepository<TEntity> where TEntity : class, IEntity<Guid>
{
    private readonly Dictionary<Guid, TEntity> _entities = new();

    public Task<IReadOnlyCollection<TEntity>> GetAllAsync() =>
        Task.FromResult<IReadOnlyCollection<TEntity>>(_entities.Values.ToArray());

    public Task<TEntity?> GetByIdAsync(Guid entityId)
    {
        _entities.TryGetValue(entityId, out var entity);
        return Task.FromResult(entity);
    }

    public Task<bool> ExistsAsync(Guid entityId) =>
        Task.FromResult(_entities.ContainsKey(entityId));

    public Task SaveAsync(TEntity entity)
    {
        _entities[entity.Id] = entity;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid entityId)
    {
        _entities.Remove(entityId);
        return Task.CompletedTask;
    }
}
