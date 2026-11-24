namespace CarWash.Application;

/// <summary>Операция нарушает связь между записями.</summary>
public sealed class ResourceConflictException(string message) : Exception(message);
