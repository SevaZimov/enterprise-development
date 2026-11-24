namespace CarWash.Tests;

/// <summary>Тесты запросов к данным автомойки.</summary>
public class AnalyticsTests(CarWashFixture fixture) : IClassFixture<CarWashFixture>
{
    [Fact]
    public async Task TopClientsAreOrderedByVisitCountThenName()
    {
        int[] expectedIds = [1, 2, 3, 5, 6];
        int[] expectedCounts = [3, 2, 2, 1, 1];

        var actual = await fixture.Analytics.GetTopClientsAsync(TestContext.Current.CancellationToken);

        Assert.Equal(expectedIds, actual.Select(item => item.ClientId));
        Assert.Equal(expectedCounts, actual.Select(item => item.VisitCount));
    }

    /// <summary>В момент окончания мойки машина уже свободна.</summary>
    [Fact]
    public async Task VehiclesInWashUseCurrentTimeByDefault()
    {
        int[] expectedIds = [8, 5, 3, 4];

        var actual = await fixture.Analytics.GetVehiclesInWashAsync(null, TestContext.Current.CancellationToken);

        Assert.Equal(expectedIds, actual.Select(vehicle => vehicle.Id));
    }

    [Fact]
    public async Task TopServicesAreOrderedByUsageThenName()
    {
        int[] expectedIds = [1, 4, 6, 3, 7];
        int[] expectedCounts = [3, 2, 2, 1, 1];

        var actual = await fixture.Analytics.GetTopServicesAsync(TestContext.Current.CancellationToken);

        Assert.Equal(expectedIds, actual.Select(item => item.ServiceId));
        Assert.Equal(expectedCounts, actual.Select(item => item.UsageCount));
    }

    /// <summary>Учитываются и будущие записи в бокс.</summary>
    [Fact]
    public async Task BoxIsFreeAfterItsLastBooking()
    {
        DateTimeOffset expectedTime = new(2026, 9, 14, 11, 30, 0, fixture.ReferenceTime.Offset);

        var actual = await fixture.Analytics.GetBoxAvailabilityAsync(1, TestContext.Current.CancellationToken);

        Assert.Equal(1, actual.BoxNumber);
        Assert.Equal(expectedTime, actual.FreeFrom);
    }

    [Fact]
    public async Task RevenueIncludesServicesWithoutOrders()
    {
        decimal[] expected = [1800m, 800m, 1500m, 2400m, 0m, 800m, 700m, 0m, 0m, 0m];

        var actual = await fixture.Analytics.GetRevenueAsync(TestContext.Current.CancellationToken);

        Assert.Equal(expected, actual.Select(item => item.Revenue));
    }
}
