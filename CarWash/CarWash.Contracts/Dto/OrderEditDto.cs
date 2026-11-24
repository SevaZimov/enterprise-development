using System.ComponentModel.DataAnnotations;

namespace CarWash.Contracts.Dto;

/// <summary>Кого и когда записать на мойку.</summary>
public sealed record OrderEditDto
{
    /// <summary>Номер клиента.</summary>
    [Range(1, int.MaxValue)]
    public required int ClientId { get; init; }

    /// <summary>Номер автомобиля этого клиента.</summary>
    [Range(1, int.MaxValue)]
    public required int VehicleId { get; init; }

    /// <summary>Номер услуги из каталога.</summary>
    [Range(1, int.MaxValue)]
    public required int ServiceId { get; init; }

    /// <summary>Время начала с часовым смещением.</summary>
    public required DateTimeOffset ServiceStartTime { get; init; }

    /// <summary>Номер бокса. Нумерация с единицы.</summary>
    [Range(1, int.MaxValue)]
    public required int BoxNumber { get; init; }
}
