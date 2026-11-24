using CarWash.Contracts.Crud;

namespace CarWash.Contracts.Dto;

/// <summary>Запись в расписании.</summary>
public sealed record OrderDto(int Id, int ClientId, int VehicleId, int ServiceId, DateTimeOffset ServiceStartTime, int BoxNumber)
    : IEntityDto;
