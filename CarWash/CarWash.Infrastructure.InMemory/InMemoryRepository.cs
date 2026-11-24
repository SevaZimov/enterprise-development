using System.Linq.Expressions;
using CarWash.Domain;

namespace CarWash.Infrastructure.InMemory;

/// <summary>Хранилище, которое живёт до остановки сервера.</summary>
public sealed class InMemoryRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
{
    private readonly Lock _gate = new();
    private readonly Dictionary<int, TEntity> _items;
    private int _lastId;

    public InMemoryRepository(IEnumerable<TEntity> seed)
    {
        _items = seed.ToDictionary(entity => entity.Id);
        _lastId = _items.Keys.DefaultIfEmpty().Max();
    }

    public Task<IReadOnlyList<TEntity>> ListAsync(
        Expression<Func<TEntity, bool>>? filter,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (_gate)
        {
            IEnumerable<TEntity> items = _items.Values;
            if (filter is not null)
            {
                items = items.Where(filter.Compile());
            }

            return Task.FromResult<IReadOnlyList<TEntity>>([.. items.OrderBy(entity => entity.Id)]);
        }
    }

    public Task<TEntity?> FindAsync(int id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (_gate)
        {
            return Task.FromResult(_items.GetValueOrDefault(id));
        }
    }

    public Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (_gate)
        {
            entity.Id = checked(++_lastId);
            _items.Add(entity.Id, entity);
            return Task.FromResult(entity);
        }
    }

    public Task<TEntity?> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (_gate)
        {
            if (!_items.ContainsKey(entity.Id))
            {
                return Task.FromResult<TEntity?>(null);
            }

            _items[entity.Id] = entity;
            return Task.FromResult<TEntity?>(entity);
        }
    }

    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (_gate)
        {
            return Task.FromResult(_items.Remove(id));
        }
    }
}
