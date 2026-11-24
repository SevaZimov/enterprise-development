namespace CarWash.Domain;

/// <summary>Клиент автомойки.</summary>
public class Client : IEntity
{
    public int Id { get; set; }

    public required string FullName { get; init; }

    public required string PhoneNumber { get; init; }
}
