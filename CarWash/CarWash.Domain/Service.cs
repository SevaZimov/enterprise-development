namespace CarWash.Domain;

/// <summary>Услуга автомойки.</summary>
public class Service
{
    public required string Name { get; set; }

    public required decimal Price { get; set; }

    /// <summary>Плановая длительность в минутах.</summary>
    public required int DurationMinutes { get; set; }

    public required VehicleCategory Category { get; set; }
}
