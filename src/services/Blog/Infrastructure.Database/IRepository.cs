using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Database;

public interface IRepository<T>
where T : class
{
    ValueTask<EntityEntry<T>> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    EntityEntry<T> Update(T entity);
    EntityEntry<T> Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
}