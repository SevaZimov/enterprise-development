using System.Linq.Expressions;

namespace CarWash.Domain;

/// <summary>Хранилище сущностей.</summary>
public interface IRepository<TEntity> where TEntity : class, IEntity
{
    public Task<IReadOnlyList<TEntity>> ListAsync(
        Expression<Func<TEntity, bool>>? filter,
        CancellationToken cancellationToken);

    public Task<TEntity?> FindAsync(int id, CancellationToken cancellationToken);

    public Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);

    public Task<TEntity?> UpdateAsync(TEntity entity, CancellationToken cancellationToken);

    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}
