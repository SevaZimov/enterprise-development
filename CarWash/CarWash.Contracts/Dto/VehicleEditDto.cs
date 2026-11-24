using System.ComponentModel.DataAnnotations;
using CarWash.Domain;

namespace CarWash.Contracts.Dto;

/// <summary>Данные автомобиля.</summary>
public sealed record VehicleEditDto
{
    [Range(1, int.MaxValue)]
    public required int ClientId { get; init; }

    [Required, MaxLength(20)]
    public required string GovNumber { get; init; }

    [Required, MaxLength(100)]
    public required string Brand { get; init; }

    [EnumDataType(typeof(VehicleCategory))]
    public required VehicleCategory Category { get; init; }
}
