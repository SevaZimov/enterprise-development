using CarWash.Domain;
using CarWash.Infrastructure.InMemory;

namespace CarWash.Tests;

/// <summary>Данные для тестов автомойки.</summary>
public class CarWashFixture
{
    private readonly CarWashSeed _seed = new();

    public DateTimeOffset ReferenceTime => _seed.ReferenceTime;

    public IReadOnlyList<Vehicle> Vehicles => _seed.Vehicles;

    public IReadOnlyList<Client> Clients => _seed.Clients;

    public IReadOnlyList<Service> Services => _seed.Services;

    public IReadOnlyList<Order> Orders => _seed.Orders;
}
