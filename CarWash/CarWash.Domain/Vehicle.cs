namespace CarWash.Domain;

/// <summary>Транспорт клиента.</summary>
public class Vehicle
{
    public required string GovNumber { get; init; }

    public required string Brand { get; init; }

    public required VehicleCategory Category { get; init; }
}
