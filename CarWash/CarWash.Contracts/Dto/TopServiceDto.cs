namespace CarWash.Contracts.Dto;

/// <summary>Услуга и число заказов.</summary>
public sealed record TopServiceDto(int ServiceId, string Name, int UsageCount);
