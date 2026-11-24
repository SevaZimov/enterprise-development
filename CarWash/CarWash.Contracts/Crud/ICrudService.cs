namespace CarWash.Contracts.Crud;

/// <summary>Операции с одним видом записей.</summary>
public interface ICrudService<TDto, in TEditDto> where TDto : class, IEntityDto
{
    public Task<IReadOnlyList<TDto>> ListAsync(CancellationToken cancellationToken);

    public Task<TDto?> FindAsync(int id, CancellationToken cancellationToken);

    public Task<TDto> CreateAsync(TEditDto request, CancellationToken cancellationToken);

    public Task<TDto?> UpdateAsync(int id, TEditDto request, CancellationToken cancellationToken);

    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}
