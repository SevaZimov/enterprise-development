using CarWash.Contracts.Dto;
using CarWash.Domain;

namespace CarWash.Application.Services;

/// <summary>Операции с клиентами.</summary>
public sealed class ClientService(
    IRepository<Client> clients,
    IRepository<Vehicle> vehicles,
    IRepository<Order> orders,
    CarWashGate gate)
    : CrudService<Client, ClientDto, ClientEditDto>(clients, gate)
{
    protected override Client ToEntity(int id, ClientEditDto request)
    {
        return new() { Id = id, FullName = request.FullName.Trim(), PhoneNumber = request.PhoneNumber.Trim() };
    }

    protected override ClientDto ToDto(Client entity)
    {
        return new(entity.Id, entity.FullName, entity.PhoneNumber);
    }

    protected override async Task ValidateRemovalAsync(Client entity, CancellationToken cancellationToken)
    {
        var ownedVehicles = await vehicles.ListAsync(vehicle => vehicle.ClientId == entity.Id, cancellationToken);
        var bookings = await orders.ListAsync(order => order.ClientId == entity.Id, cancellationToken);
        if (ownedVehicles.Count > 0 || bookings.Count > 0)
        {
            throw new ResourceConflictException("У клиента есть автомобили или заказы.");
        }
    }
}
