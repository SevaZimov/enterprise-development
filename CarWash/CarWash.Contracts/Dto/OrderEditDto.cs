using System.ComponentModel.DataAnnotations;

namespace CarWash.Contracts.Dto;

/// <summary>Кого и когда записать на мойку.</summary>
public sealed record OrderEditDto
{
    [Range(1, int.MaxValue)]
    public required int ClientId { get; init; }

    [Range(1, int.MaxValue)]
    public required int VehicleId { get; init; }

    [Range(1, int.MaxValue)]
    public required int ServiceId { get; init; }

    public required DateTimeOffset ServiceStartTime { get; init; }

    [Range(1, int.MaxValue)]
    public required int BoxNumber { get; init; }
}
