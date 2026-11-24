using System.ComponentModel.DataAnnotations;
using CarWash.Domain;

namespace CarWash.Contracts.Dto;

/// <summary>Данные для записи в каталог услуг.</summary>
public sealed record ServiceEditDto
{
    /// <summary>Название в каталоге.</summary>
    [Required, MaxLength(200)]
    public required string Name { get; init; }

    /// <summary>Цена в рублях.</summary>
    [Range(typeof(decimal), "0", "1000000000", ParseLimitsInInvariantCulture = true)]
    public required decimal Price { get; init; }

    /// <summary>Плановая длительность в минутах.</summary>
    [Range(1, int.MaxValue)]
    public required int DurationMinutes { get; init; }

    /// <summary>Для какого транспорта подходит услуга.</summary>
    [EnumDataType(typeof(VehicleCategory))]
    public required VehicleCategory Category { get; init; }
}
