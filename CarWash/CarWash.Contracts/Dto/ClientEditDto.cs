using System.ComponentModel.DataAnnotations;

namespace CarWash.Contracts.Dto;

/// <summary>Имя и телефон клиента.</summary>
public sealed record ClientEditDto
{
    /// <summary>Имя и фамилия клиента.</summary>
    [Required, MaxLength(200)]
    public required string FullName { get; init; }

    /// <summary>Номер телефона клиента.</summary>
    [Required, Phone, MaxLength(25)]
    public required string PhoneNumber { get; init; }
}
