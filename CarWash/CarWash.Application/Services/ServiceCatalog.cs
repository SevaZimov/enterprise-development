using CarWash.Contracts.Dto;
using CarWash.Domain;

namespace CarWash.Application.Services;

/// <summary>Операции с каталогом услуг.</summary>
public sealed class ServiceCatalog(IRepository<Service> services, IRepository<Order> orders, CarWashGate gate)
    : CrudService<Service, ServiceDto, ServiceEditDto>(services, gate)
{
    protected override Service ToEntity(int id, ServiceEditDto request)
    {
        return new()
        {
            Id = id,
            Name = request.Name.Trim(),
            Price = request.Price,
            DurationMinutes = request.DurationMinutes,
            Category = request.Category
        };
    }

    protected override ServiceDto ToDto(Service entity)
    {
        return new(entity.Id, entity.Name, entity.Price, entity.DurationMinutes, entity.Category);
    }

    protected override async Task ValidateChangeAsync(Service entity, Service? previous, CancellationToken cancellationToken)
    {
        if (previous is null || (previous.Category == entity.Category && previous.DurationMinutes == entity.DurationMinutes))
        {
            return;
        }

        var bookings = await orders.ListAsync(order => order.ServiceId == entity.Id, cancellationToken);
        if (bookings.Count > 0)
        {
            throw new ResourceConflictException("У услуги с заказами нельзя менять категорию или длительность.");
        }
    }

    protected override async Task ValidateRemovalAsync(Service entity, CancellationToken cancellationToken)
    {
        var bookings = await orders.ListAsync(order => order.ServiceId == entity.Id, cancellationToken);
        if (bookings.Count > 0)
        {
            throw new ResourceConflictException("У услуги есть заказы.");
        }
    }
}
