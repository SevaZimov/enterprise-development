namespace CarWash.Contracts.Dto;

/// <summary>Клиент и число визитов.</summary>
public sealed record TopClientDto(int ClientId, string FullName, int VisitCount);
