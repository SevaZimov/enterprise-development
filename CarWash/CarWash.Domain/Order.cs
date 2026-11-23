namespace CarWash.Domain;

/// <summary>Запись на мойку.</summary>
public class Order
{
    public required Client Client { get; init; }

    public required Vehicle Vehicle { get; init; }

    public required Service Service { get; init; }

    public required DateTime ServiceStartTime { get; init; }

    public required int BoxNumber { get; init; }
}
