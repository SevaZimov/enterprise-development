namespace CarWash.Domain;

/// <summary>Запись на мойку.</summary>
public class Order : IEntity
{
    public int Id { get; set; }

    public required int ClientId { get; init; }

    public required int VehicleId { get; init; }

    public required int ServiceId { get; init; }

    public required DateTimeOffset ServiceStartTime { get; init; }

    public required int BoxNumber { get; init; }
}
