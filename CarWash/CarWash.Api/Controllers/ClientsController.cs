using CarWash.Contracts.Crud;
using CarWash.Contracts.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CarWash.Api.Controllers;

/// <summary>Клиенты автомойки.</summary>
[Route("api/clients")]
public sealed class ClientsController(ICrudService<ClientDto, ClientEditDto> service)
    : CrudController<ClientDto, ClientEditDto>(service);
