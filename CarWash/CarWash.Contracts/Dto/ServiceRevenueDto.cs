namespace CarWash.Contracts.Dto;

/// <summary>Выручка по услуге.</summary>
public sealed record ServiceRevenueDto(int ServiceId, string Name, decimal Revenue);
