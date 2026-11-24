using CarWash.Api;
using CarWash.Application;
using CarWash.Application.Services;
using CarWash.Contracts.Crud;
using CarWash.Contracts.Dto;
using CarWash.Domain;
using CarWash.Infrastructure.InMemory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();

var seed = new CarWashSeed();
builder.Services.AddSingleton<IRepository<Client>>(new InMemoryRepository<Client>(seed.Clients));
builder.Services.AddSingleton<IRepository<Vehicle>>(new InMemoryRepository<Vehicle>(seed.Vehicles));
builder.Services.AddSingleton<IRepository<Service>>(new InMemoryRepository<Service>(seed.Services));
builder.Services.AddSingleton<IRepository<Order>>(new InMemoryRepository<Order>(seed.Orders));
builder.Services.AddSingleton<CarWashGate>();
builder.Services.AddScoped<ICrudService<ServiceDto, ServiceEditDto>, ServiceCatalog>();

var app = builder.Build();

app.UseExceptionHandler();
app.MapControllers();
app.Run();
