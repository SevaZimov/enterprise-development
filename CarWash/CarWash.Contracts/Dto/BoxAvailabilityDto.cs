namespace CarWash.Contracts.Dto;

/// <summary>Время окончания всех записей в боксе.</summary>
public sealed record BoxAvailabilityDto(int BoxNumber, DateTimeOffset? FreeFrom);
