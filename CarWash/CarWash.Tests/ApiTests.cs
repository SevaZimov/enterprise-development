using System.Net;
using System.Net.Http.Json;
using CarWash.Api.Controllers;
using CarWash.Contracts.Dto;
using CarWash.Domain;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CarWash.Tests;

/// <summary>Проверки запросов к серверу.</summary>
public class ApiTests
{
    [Fact]
    public async Task ServiceCanBeCreatedUpdatedAndDeleted()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        await using var factory = new WebApplicationFactory<ServicesController>();
        using var client = factory.CreateClient();
        ServiceEditDto request = new()
        {
            Name = "Мойка стёкол",
            Price = 450m,
            DurationMinutes = 15,
            Category = VehicleCategory.Car
        };

        using var created = await client.PostAsJsonAsync("/api/services", request, cancellationToken);
        var service = await created.Content.ReadFromJsonAsync<ServiceDto>(cancellationToken);
        Assert.Equal(HttpStatusCode.Created, created.StatusCode);
        Assert.NotNull(service);
        Assert.True(service.Id > 10);
        var location = created.Headers.Location;
        Assert.NotNull(location);
        var saved = await client.GetFromJsonAsync<ServiceDto>(location, cancellationToken);
        Assert.Equal(service, saved);

        using var updated = await client.PutAsJsonAsync(location, request with { Price = 500m }, cancellationToken);
        var savedUpdate = await updated.Content.ReadFromJsonAsync<ServiceDto>(cancellationToken);
        Assert.Equal(HttpStatusCode.OK, updated.StatusCode);
        Assert.Equal(service with { Price = 500m }, savedUpdate);

        using var removed = await client.DeleteAsync(location, cancellationToken);
        using var missing = await client.GetAsync(location, cancellationToken);
        Assert.Equal(HttpStatusCode.NoContent, removed.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, missing.StatusCode);
    }

    [Fact]
    public async Task OrderRejectsAWrongVehicleOwner()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        await using var factory = new WebApplicationFactory<ServicesController>();
        using var client = factory.CreateClient();
        OrderEditDto request = new()
        {
            ClientId = 2,
            VehicleId = 1,
            ServiceId = 1,
            ServiceStartTime = new DateTimeOffset(2026, 9, 15, 10, 0, 0, TimeSpan.FromHours(4)),
            BoxNumber = 1
        };

        using var response = await client.PostAsJsonAsync("/api/orders", request, cancellationToken);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ServiceWithOrdersCannotBeDeleted()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        await using var factory = new WebApplicationFactory<ServicesController>();
        using var client = factory.CreateClient();

        using var removed = await client.DeleteAsync("/api/services/1", cancellationToken);
        using var stillExists = await client.GetAsync("/api/services/1", cancellationToken);

        Assert.Equal(HttpStatusCode.Conflict, removed.StatusCode);
        Assert.Equal(HttpStatusCode.OK, stillExists.StatusCode);
    }
}
