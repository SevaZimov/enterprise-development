namespace CarWash.Domain;

/// <summary>Клиент автомойки.</summary>
public class Client
{
    public required string FullName { get; set; }

    public required string PhoneNumber { get; set; }

    public required List<Vehicle> Vehicles { get; set; }
}
