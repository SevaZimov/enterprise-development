using CarWash.Domain;

namespace CarWash.Tests;

/// <summary>Тесты запросов к данным автомойки.</summary>
public class AnalyticsTests(CarWashFixture fixture) : IClassFixture<CarWashFixture>
{
    /// <summary>Каждый заказ считается одним визитом.</summary>
    [Fact]
    public void Top5ClientsShouldReturnClientsOrderedByVisitsThenName()
    {
        Client[] expectedClients =
        [
            fixture.Clients[0],
            fixture.Clients[1],
            fixture.Clients[2],
            fixture.Clients[4],
            fixture.Clients[5]
        ];
        int[] expectedVisitCounts = [3, 2, 2, 1, 1];

        var actual = fixture.Orders
            .GroupBy(order => order.Client)
            .Select(group => new
            {
                Client = group.Key,
                VisitCount = group.Count()
            })
            .OrderByDescending(item => item.VisitCount)
            .ThenBy(item => item.Client.FullName, StringComparer.Ordinal)
            .Take(5)
            .ToArray();

        Assert.Equal(expectedClients, [.. actual.Select(item => item.Client)]);
        Assert.Equal(expectedVisitCounts, actual.Select(item => item.VisitCount).ToArray());
    }

    /// <summary>В момент окончания мойки машина уже свободна.</summary>
    [Fact]
    public void VehiclesOnWashNowShouldReturnVehiclesBeingWashed()
    {
        var timeNow = fixture.ReferenceTime;
        Vehicle[] expectedVehicles =
        [
            fixture.Vehicles[7],
            fixture.Vehicles[4],
            fixture.Vehicles[2],
            fixture.Vehicles[3]
        ];

        var actual = fixture.Orders
            .Where(order => order.ServiceStartTime <= timeNow
                && order.ServiceStartTime.AddMinutes(order.Service.DurationMinutes) > timeNow)
            .Select(order => order.Vehicle)
            .ToArray();

        Assert.Equal(expectedVehicles, actual);
    }

    /// <summary>При равном числе заказов услуги идут по названию.</summary>
    [Fact]
    public void Top5ServicesShouldReturnServicesOrderedByUsageThenName()
    {
        Service[] expectedServices =
        [
            fixture.Services[0],
            fixture.Services[3],
            fixture.Services[5],
            fixture.Services[2],
            fixture.Services[6]
        ];
        int[] expectedUsageCounts = [3, 2, 2, 1, 1];

        var actual = fixture.Orders
            .GroupBy(order => order.Service)
            .Select(group => new
            {
                Service = group.Key,
                UsageCount = group.Count()
            })
            .OrderByDescending(item => item.UsageCount)
            .ThenBy(item => item.Service.Name, StringComparer.Ordinal)
            .Take(5)
            .ToArray();

        Assert.Equal(expectedServices, [.. actual.Select(item => item.Service)]);
        Assert.Equal(expectedUsageCounts, actual.Select(item => item.UsageCount).ToArray());
    }

    /// <summary>Учитываются и будущие записи в бокс.</summary>
    [Fact]
    public void TimeWhenBoxBecomesFreeShouldReturnLastScheduledEndTime()
    {
        var boxNumber = 1;
        DateTime expectedTime = new(2026, 9, 14, 11, 30, 0);

        var actual = fixture.Orders
            .Where(order => order.BoxNumber == boxNumber)
            .OrderByDescending(order => order.ServiceStartTime)
            .Select(order => order.ServiceStartTime.AddMinutes(order.Service.DurationMinutes))
            .FirstOrDefault();

        Assert.Equal(expectedTime, actual);
    }

    /// <summary>Услуга без заказов даёт нулевую выручку.</summary>
    [Fact]
    public void TotalRevenueForEachServiceShouldIncludeServicesWithoutOrders()
    {
        decimal[] expectedRevenues =
        [
            1800m,
            800m,
            1500m,
            2400m,
            0m,
            800m,
            700m,
            0m,
            0m,
            0m
        ];

        var actual = fixture.Services
            .Select(currentService => fixture.Orders
                .Where(order => order.Service == currentService)
                .Sum(order => order.Service.Price))
            .ToArray();

        Assert.Equal(expectedRevenues, actual);
    }
}
