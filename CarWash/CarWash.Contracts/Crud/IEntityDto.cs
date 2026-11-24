namespace CarWash.Contracts.Crud;

/// <summary>Запись в ответе API.</summary>
public interface IEntityDto
{
    /// <summary>Номер, который выдал сервер.</summary>
    public int Id { get; }
}
