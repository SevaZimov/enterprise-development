using CarWash.Contracts.Crud;
using CarWash.Domain;

namespace CarWash.Contracts.Dto;

/// <summary>Автомобиль и номер его владельца.</summary>
public sealed record VehicleDto(int Id, int ClientId, string GovNumber, string Brand, VehicleCategory Category)
    : IEntityDto;
