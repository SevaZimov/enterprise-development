using CarWash.Contracts.Dto;

namespace CarWash.Contracts.Analytics;

/// <summary>Отчёты автомойки.</summary>
public interface IAnalyticsService
{
    /// <summary>До пяти клиентов по числу визитов.</summary>
    public Task<IReadOnlyList<TopClientDto>> GetTopClientsAsync(CancellationToken cancellationToken);

    /// <summary>Текущая загрузка, если время не задано.</summary>
    public Task<IReadOnlyList<VehicleDto>> GetVehiclesInWashAsync(DateTimeOffset? at, CancellationToken cancellationToken);

    /// <summary>До пяти услуг по числу заказов.</summary>
    public Task<IReadOnlyList<TopServiceDto>> GetTopServicesAsync(CancellationToken cancellationToken);

    /// <summary>Конец последней записи в боксе.</summary>
    public Task<BoxAvailabilityDto> GetBoxAvailabilityAsync(int boxNumber, CancellationToken cancellationToken);

    /// <summary>Выручка по каждой услуге по текущей цене.</summary>
    public Task<IReadOnlyList<ServiceRevenueDto>> GetRevenueAsync(CancellationToken cancellationToken);
}
