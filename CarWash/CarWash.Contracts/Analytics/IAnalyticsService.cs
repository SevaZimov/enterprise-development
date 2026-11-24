using CarWash.Contracts.Dto;

namespace CarWash.Contracts.Analytics;

/// <summary>Отчёты автомойки.</summary>
public interface IAnalyticsService
{
    public Task<IReadOnlyList<TopClientDto>> GetTopClientsAsync(CancellationToken cancellationToken);

    public Task<IReadOnlyList<VehicleDto>> GetVehiclesInWashAsync(DateTimeOffset? at, CancellationToken cancellationToken);

    public Task<IReadOnlyList<TopServiceDto>> GetTopServicesAsync(CancellationToken cancellationToken);

    public Task<BoxAvailabilityDto> GetBoxAvailabilityAsync(int boxNumber, CancellationToken cancellationToken);

    public Task<IReadOnlyList<ServiceRevenueDto>> GetRevenueAsync(CancellationToken cancellationToken);
}
