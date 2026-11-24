using System.ComponentModel.DataAnnotations;
using CarWash.Contracts.Dto;
using CarWash.Domain;

namespace CarWash.Application.Services;

/// <summary>Операции с автомобилями.</summary>
public sealed class VehicleService(
    IRepository<Vehicle> vehicles,
    IRepository<Client> clients,
    IRepository<Order> orders,
    CarWashGate gate)
    : CrudService<Vehicle, VehicleDto, VehicleEditDto>(vehicles, gate)
{
    protected override Vehicle ToEntity(int id, VehicleEditDto request)
    {
        return new()
        {
            Id = id,
            ClientId = request.ClientId,
            GovNumber = request.GovNumber.Trim().ToUpperInvariant(),
            Brand = request.Brand.Trim(),
            Category = request.Category
        };
    }

    protected override VehicleDto ToDto(Vehicle entity)
    {
        return new(entity.Id, entity.ClientId, entity.GovNumber, entity.Brand, entity.Category);
    }

    protected override async Task ValidateChangeAsync(Vehicle entity, Vehicle? previous, CancellationToken cancellationToken)
    {
        if (await clients.FindAsync(entity.ClientId, cancellationToken) is null)
        {
            throw new ValidationException("Клиент с таким номером не найден.");
        }

        var matching = await Repository.ListAsync(
            vehicle => vehicle.Id != entity.Id && vehicle.GovNumber == entity.GovNumber,
            cancellationToken);
        if (matching.Count > 0)
        {
            throw new ResourceConflictException("Автомобиль с таким госномером уже есть.");
        }

        if (previous is not null && (previous.ClientId != entity.ClientId || previous.Category != entity.Category))
        {
            var bookings = await orders.ListAsync(order => order.VehicleId == entity.Id, cancellationToken);
            if (bookings.Count > 0)
            {
                throw new ResourceConflictException("У автомобиля с заказами нельзя менять владельца или категорию.");
            }
        }
    }

    protected override async Task ValidateRemovalAsync(Vehicle entity, CancellationToken cancellationToken)
    {
        var bookings = await orders.ListAsync(order => order.VehicleId == entity.Id, cancellationToken);
        if (bookings.Count > 0)
        {
            throw new ResourceConflictException("У автомобиля есть заказы.");
        }
    }
}
