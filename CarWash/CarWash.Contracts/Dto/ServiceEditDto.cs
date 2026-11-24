using System.ComponentModel.DataAnnotations;
using CarWash.Domain;

namespace CarWash.Contracts.Dto;

/// <summary>Данные для записи в каталог услуг.</summary>
public sealed record ServiceEditDto
{
    [Required, MaxLength(200)]
    public required string Name { get; init; }

    [Range(typeof(decimal), "0", "1000000000", ParseLimitsInInvariantCulture = true)]
    public required decimal Price { get; init; }

    [Range(1, int.MaxValue)]
    public required int DurationMinutes { get; init; }

    [EnumDataType(typeof(VehicleCategory))]
    public required VehicleCategory Category { get; init; }
}
