using System.ComponentModel.DataAnnotations;
using CarWash.Contracts.Crud;
using CarWash.Domain;

namespace CarWash.Application.Services;

/// <summary>Общий порядок чтения, проверки и записи данных.</summary>
public abstract class CrudService<TEntity, TDto, TEditDto>(IRepository<TEntity> repository, CarWashGate gate)
    : ICrudService<TDto, TEditDto>
    where TEntity : class, IEntity
    where TDto : class, IEntityDto
    where TEditDto : class
{
    public async Task<IReadOnlyList<TDto>> ListAsync(CancellationToken cancellationToken)
    {
        var items = await repository.ListAsync(null, cancellationToken);
        return [.. items.Select(ToDto)];
    }

    public async Task<TDto?> FindAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await repository.FindAsync(id, cancellationToken);
        return entity is null ? null : ToDto(entity);
    }

    public Task<TDto> CreateAsync(TEditDto request, CancellationToken cancellationToken)
    {
        return gate.RunAsync(async () =>
        {
            Validator.ValidateObject(request, new ValidationContext(request), validateAllProperties: true);
            var entity = ToEntity(0, request);
            await ValidateChangeAsync(entity, null, cancellationToken);
            return ToDto(await repository.AddAsync(entity, cancellationToken));
        }, cancellationToken);
    }

    public Task<TDto?> UpdateAsync(int id, TEditDto request, CancellationToken cancellationToken)
    {
        return gate.RunAsync<TDto?>(async () =>
        {
            var previous = await repository.FindAsync(id, cancellationToken);
            if (previous is null)
            {
                return null;
            }

            Validator.ValidateObject(request, new ValidationContext(request), validateAllProperties: true);
            var entity = ToEntity(id, request);
            await ValidateChangeAsync(entity, previous, cancellationToken);
            var updated = await repository.UpdateAsync(entity, cancellationToken);
            return updated is null ? null : ToDto(updated);
        }, cancellationToken);
    }

    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        return gate.RunAsync(async () =>
        {
            var entity = await repository.FindAsync(id, cancellationToken);
            if (entity is null)
            {
                return false;
            }

            await ValidateRemovalAsync(entity, cancellationToken);
            return await repository.DeleteAsync(id, cancellationToken);
        }, cancellationToken);
    }

    protected abstract TEntity ToEntity(int id, TEditDto request);

    protected abstract TDto ToDto(TEntity entity);

    protected virtual Task ValidateChangeAsync(TEntity entity, TEntity? previous, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    protected virtual Task ValidateRemovalAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
