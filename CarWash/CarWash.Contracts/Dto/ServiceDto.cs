using CarWash.Contracts.Crud;
using CarWash.Domain;

namespace CarWash.Contracts.Dto;

/// <summary>Услуга из каталога.</summary>
public sealed record ServiceDto(int Id, string Name, decimal Price, int DurationMinutes, VehicleCategory Category)
    : IEntityDto;
