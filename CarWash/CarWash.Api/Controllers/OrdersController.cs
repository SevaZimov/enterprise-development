using CarWash.Contracts.Crud;
using CarWash.Contracts.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CarWash.Api.Controllers;

/// <summary>Записи на мойку.</summary>
[Route("api/orders")]
public sealed class OrdersController(ICrudService<OrderDto, OrderEditDto> service)
    : CrudController<OrderDto, OrderEditDto>(service);
