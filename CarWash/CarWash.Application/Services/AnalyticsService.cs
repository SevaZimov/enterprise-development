using CarWash.Contracts.Analytics;
using CarWash.Contracts.Dto;
using CarWash.Domain;

namespace CarWash.Application.Services;

/// <summary>Считает показатели по заказам.</summary>
public sealed class AnalyticsService(
    IRepository<Order> orders,
    IRepository<Client> clients,
    IRepository<Vehicle> vehicles,
    IRepository<Service> services,
    TimeProvider clock,
    CarWashGate gate) : IAnalyticsService
{
    public Task<IReadOnlyList<TopClientDto>> GetTopClientsAsync(CancellationToken cancellationToken)
    {
        return gate.RunAsync<IReadOnlyList<TopClientDto>>(async () =>
        {
            var allOrders = await orders.ListAsync(null, cancellationToken);
            var visits = allOrders.GroupBy(order => order.ClientId).ToDictionary(group => group.Key, group => group.Count());
            var clientIds = visits.Keys.ToArray();
            var visitors = await clients.ListAsync(client => clientIds.Contains(client.Id), cancellationToken);

            return
            [
                .. visitors.Select(client => new TopClientDto(client.Id, client.FullName, visits[client.Id]))
                    .OrderByDescending(item => item.VisitCount)
                    .ThenBy(item => item.FullName, StringComparer.Ordinal)
                    .ThenBy(item => item.ClientId)
                    .Take(5)
            ];
        }, cancellationToken);
    }

    public Task<IReadOnlyList<VehicleDto>> GetVehiclesInWashAsync(DateTimeOffset? at, CancellationToken cancellationToken)
    {
        return gate.RunAsync<IReadOnlyList<VehicleDto>>(async () =>
        {
            var time = at ?? clock.GetUtcNow();
            var startedOrders = await orders.ListAsync(order => order.ServiceStartTime <= time, cancellationToken);
            var durations = await GetDurationsAsync(startedOrders, cancellationToken);
            var activeVehicleIds = startedOrders
                .Where(order => order.ServiceStartTime.AddMinutes(durations[order.ServiceId]) > time)
                .Select(order => order.VehicleId)
                .Distinct()
                .ToArray();
            var activeVehicles = await vehicles.ListAsync(
                vehicle => activeVehicleIds.Contains(vehicle.Id),
                cancellationToken);
            var vehiclesById = activeVehicles.ToDictionary(vehicle => vehicle.Id);

            return
            [
                .. activeVehicleIds.Select(id =>
                {
                    var vehicle = vehiclesById[id];
                    return new VehicleDto(vehicle.Id, vehicle.ClientId, vehicle.GovNumber, vehicle.Brand, vehicle.Category);
                })
            ];
        }, cancellationToken);
    }

    public Task<IReadOnlyList<TopServiceDto>> GetTopServicesAsync(CancellationToken cancellationToken)
    {
        return gate.RunAsync<IReadOnlyList<TopServiceDto>>(async () =>
        {
            var allOrders = await orders.ListAsync(null, cancellationToken);
            var counts = allOrders.GroupBy(order => order.ServiceId).ToDictionary(group => group.Key, group => group.Count());
            var serviceIds = counts.Keys.ToArray();
            var usedServices = await services.ListAsync(service => serviceIds.Contains(service.Id), cancellationToken);

            return
            [
                .. usedServices.Select(service => new TopServiceDto(service.Id, service.Name, counts[service.Id]))
                    .OrderByDescending(item => item.UsageCount)
                    .ThenBy(item => item.Name, StringComparer.Ordinal)
                    .ThenBy(item => item.ServiceId)
                    .Take(5)
            ];
        }, cancellationToken);
    }

    public Task<BoxAvailabilityDto> GetBoxAvailabilityAsync(int boxNumber, CancellationToken cancellationToken)
    {
        return gate.RunAsync(async () =>
        {
            var bookings = await orders.ListAsync(order => order.BoxNumber == boxNumber, cancellationToken);
            if (bookings.Count == 0)
            {
                return new BoxAvailabilityDto(boxNumber, null);
            }

            var durations = await GetDurationsAsync(bookings, cancellationToken);
            var freeFrom = bookings.Max(order => order.ServiceStartTime.AddMinutes(durations[order.ServiceId]));
            return new BoxAvailabilityDto(boxNumber, freeFrom);
        }, cancellationToken);
    }

    public Task<IReadOnlyList<ServiceRevenueDto>> GetRevenueAsync(CancellationToken cancellationToken)
    {
        return gate.RunAsync<IReadOnlyList<ServiceRevenueDto>>(async () =>
        {
            var allServices = await services.ListAsync(null, cancellationToken);
            var allOrders = await orders.ListAsync(null, cancellationToken);
            var counts = allOrders.GroupBy(order => order.ServiceId).ToDictionary(group => group.Key, group => group.Count());

            return
            [
                .. allServices.Select(service =>
                    new ServiceRevenueDto(service.Id, service.Name, service.Price * counts.GetValueOrDefault(service.Id)))
            ];
        }, cancellationToken);
    }

    private async Task<Dictionary<int, int>> GetDurationsAsync(IReadOnlyList<Order> bookings, CancellationToken cancellationToken)
    {
        var serviceIds = bookings.Select(order => order.ServiceId).Distinct().ToArray();
        var bookedServices = await services.ListAsync(service => serviceIds.Contains(service.Id), cancellationToken);
        return bookedServices.ToDictionary(service => service.Id, service => service.DurationMinutes);
    }
}
