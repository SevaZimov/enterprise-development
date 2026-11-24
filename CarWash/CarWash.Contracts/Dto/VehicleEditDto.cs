using System.ComponentModel.DataAnnotations;
using CarWash.Domain;

namespace CarWash.Contracts.Dto;

/// <summary>Данные автомобиля.</summary>
public sealed record VehicleEditDto
{
    /// <summary>Номер владельца.</summary>
    [Range(1, int.MaxValue)]
    public required int ClientId { get; init; }

    /// <summary>Госномер автомобиля.</summary>
    [Required, MaxLength(20)]
    public required string GovNumber { get; init; }

    /// <summary>Марка автомобиля.</summary>
    [Required, MaxLength(100)]
    public required string Brand { get; init; }

    /// <summary>Категория транспорта.</summary>
    [EnumDataType(typeof(VehicleCategory))]
    public required VehicleCategory Category { get; init; }
}
