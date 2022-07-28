using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Database;

public abstract class BaseRepository<T> : IRepository<T>
where T : class
{
    private readonly DbSet<T> _dbSet;

    protected BaseRepository(DbSet<T> dbSet)
    {
        _dbSet = dbSet;
    }

    public virtual ValueTask<EntityEntry<T>> AddAsync(T entity, CancellationToken cancellationToken = default) => _dbSet.AddAsync(entity, cancellationToken);
    public Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default) => _dbSet.AddRangeAsync(entities, cancellationToken);
    public EntityEntry<T> Update(T entity) => _dbSet.Update(entity);
    public EntityEntry<T> Remove(T entity) => _dbSet.Remove(entity);
    public void RemoveRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);
}