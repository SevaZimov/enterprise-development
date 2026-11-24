using System.ComponentModel.DataAnnotations;

namespace CarWash.Contracts.Dto;

/// <summary>Имя и телефон клиента.</summary>
public sealed record ClientEditDto
{
    [Required, MaxLength(200)]
    public required string FullName { get; init; }

    [Required, Phone, MaxLength(25)]
    public required string PhoneNumber { get; init; }
}
