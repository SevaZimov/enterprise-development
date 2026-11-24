using System.ComponentModel.DataAnnotations;
using CarWash.Contracts.Dto;
using CarWash.Domain;

namespace CarWash.Application.Services;

/// <summary>Запись на мойку и проверка расписания.</summary>
public sealed class OrderService(
    IRepository<Order> orders,
    IRepository<Client> clients,
    IRepository<Vehicle> vehicles,
    IRepository<Service> services,
    CarWashGate gate)
    : CrudService<Order, OrderDto, OrderEditDto>(orders, gate)
{
    protected override Order ToEntity(int id, OrderEditDto request)
    {
        return new()
        {
            Id = id,
            ClientId = request.ClientId,
            VehicleId = request.VehicleId,
            ServiceId = request.ServiceId,
            ServiceStartTime = request.ServiceStartTime,
            BoxNumber = request.BoxNumber
        };
    }

    protected override OrderDto ToDto(Order entity)
    {
        return new(entity.Id, entity.ClientId, entity.VehicleId, entity.ServiceId, entity.ServiceStartTime, entity.BoxNumber);
    }

    protected override async Task ValidateChangeAsync(Order entity, Order? previous, CancellationToken cancellationToken)
    {
        _ = await clients.FindAsync(entity.ClientId, cancellationToken)
            ?? throw new ValidationException("Клиент с таким номером не найден.");

        var vehicle = await vehicles.FindAsync(entity.VehicleId, cancellationToken)
            ?? throw new ValidationException("Автомобиль с таким номером не найден.");

        var service = await services.FindAsync(entity.ServiceId, cancellationToken)
            ?? throw new ValidationException("Услуга с таким номером не найдена.");

        if (vehicle.ClientId != entity.ClientId)
        {
            throw new ValidationException("Автомобиль принадлежит другому клиенту.");
        }

        if (vehicle.Category != service.Category)
        {
            throw new ValidationException("Услуга не подходит категории автомобиля.");
        }

        var end = CalculateEnd(entity.ServiceStartTime, service.DurationMinutes);
        var nearbyOrders = await Repository.ListAsync(
            order => order.Id != entity.Id && order.ServiceStartTime < end
                && (order.BoxNumber == entity.BoxNumber || order.VehicleId == entity.VehicleId),
            cancellationToken);
        if (nearbyOrders.Count == 0)
        {
            return;
        }

        var serviceIds = nearbyOrders.Select(order => order.ServiceId).Distinct().ToArray();
        var scheduledServices = await services.ListAsync(item => serviceIds.Contains(item.Id), cancellationToken);
        var durations = scheduledServices.ToDictionary(item => item.Id, item => item.DurationMinutes);

        if (nearbyOrders.Any(order => order.ServiceStartTime.AddMinutes(durations[order.ServiceId]) > entity.ServiceStartTime))
        {
            throw new ResourceConflictException("Бокс или автомобиль уже занят в это время.");
        }
    }

    private static DateTimeOffset CalculateEnd(DateTimeOffset start, int durationMinutes)
    {
        if (start == default)
        {
            throw new ValidationException("Укажи время начала мойки.");
        }

        try
        {
            return start.AddMinutes(durationMinutes);
        }
        catch (ArgumentOutOfRangeException)
        {
            throw new ValidationException("Время окончания выходит за допустимый диапазон.");
        }
    }
}
