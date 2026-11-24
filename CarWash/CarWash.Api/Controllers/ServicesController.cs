using CarWash.Contracts.Crud;
using CarWash.Contracts.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CarWash.Api.Controllers;

/// <summary>Каталог услуг автомойки.</summary>
[Route("api/services")]
public sealed class ServicesController(ICrudService<ServiceDto, ServiceEditDto> service)
    : CrudController<ServiceDto, ServiceEditDto>(service);
