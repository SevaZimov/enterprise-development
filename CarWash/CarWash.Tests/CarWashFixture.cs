using CarWash.Application;
using CarWash.Application.Services;
using CarWash.Domain;
using CarWash.Infrastructure.InMemory;

namespace CarWash.Tests;

/// <summary>Сервис аналитики с тестовым расписанием.</summary>
public sealed class CarWashFixture : IDisposable
{
    private readonly CarWashGate _gate = new();

    public DateTimeOffset ReferenceTime { get; }

    public AnalyticsService Analytics { get; }

    public CarWashFixture()
    {
        var seed = new CarWashSeed();
        ReferenceTime = seed.ReferenceTime;
        Analytics = new(
            new InMemoryRepository<Order>(seed.Orders),
            new InMemoryRepository<Client>(seed.Clients),
            new InMemoryRepository<Vehicle>(seed.Vehicles),
            new InMemoryRepository<Service>(seed.Services),
            new FixedTimeProvider(seed.ReferenceTime),
            _gate);
    }

    public void Dispose()
    {
        _gate.Dispose();
    }

    private sealed class FixedTimeProvider(DateTimeOffset time) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow()
        {
            return time.ToUniversalTime();
        }
    }
}
