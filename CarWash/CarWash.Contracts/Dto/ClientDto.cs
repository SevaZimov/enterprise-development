using CarWash.Contracts.Crud;

namespace CarWash.Contracts.Dto;

/// <summary>Клиент автомойки.</summary>
public sealed record ClientDto(int Id, string FullName, string PhoneNumber) : IEntityDto;
