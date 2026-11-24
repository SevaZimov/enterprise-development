namespace CarWash.Domain;

/// <summary>Транспорт клиента.</summary>
public class Vehicle : IEntity
{
    public int Id { get; set; }

    public required int ClientId { get; init; }

    public required string GovNumber { get; init; }

    public required string Brand { get; init; }

    public required VehicleCategory Category { get; init; }
}
