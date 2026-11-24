namespace CarWash.Contracts.Crud;

/// <summary>Операции с одним видом записей.</summary>
public interface ICrudService<TDto, in TEditDto> where TDto : class, IEntityDto
{
    /// <summary>Все записи в порядке ID.</summary>
    public Task<IReadOnlyList<TDto>> ListAsync(CancellationToken cancellationToken);

    /// <summary>Возвращает null, если записи нет.</summary>
    public Task<TDto?> FindAsync(int id, CancellationToken cancellationToken);

    /// <summary>Создаёт запись и возвращает выданный ей ID в ответе.</summary>
    public Task<TDto> CreateAsync(TEditDto request, CancellationToken cancellationToken);

    /// <summary>Заменяет данные записи с указанным ID.</summary>
    public Task<TDto?> UpdateAsync(int id, TEditDto request, CancellationToken cancellationToken);

    /// <summary>Возвращает false, если записи уже нет.</summary>
    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}
