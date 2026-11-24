using CarWash.Contracts.Crud;
using CarWash.Contracts.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CarWash.Api.Controllers;

/// <summary>Автомобили клиентов.</summary>
[Route("api/vehicles")]
public sealed class VehiclesController(ICrudService<VehicleDto, VehicleEditDto> service)
    : CrudController<VehicleDto, VehicleEditDto>(service);
