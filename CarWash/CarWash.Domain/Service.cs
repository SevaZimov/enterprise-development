namespace CarWash.Domain;

/// <summary>Услуга автомойки.</summary>
public class Service : IEntity
{
    public int Id { get; set; }

    public required string Name { get; init; }

    public required decimal Price { get; init; }

    /// <summary>Плановая длительность в минутах.</summary>
    public required int DurationMinutes { get; init; }

    public required VehicleCategory Category { get; init; }
}
