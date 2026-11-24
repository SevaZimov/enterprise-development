using System.ComponentModel.DataAnnotations;
using CarWash.Contracts.Analytics;
using CarWash.Contracts.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CarWash.Api.Controllers;

/// <summary>Отчёты автомойки.</summary>
[ApiController]
[Route("api/analytics")]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
public sealed class AnalyticsController(IAnalyticsService analytics) : ControllerBase
{
    /// <summary>Пять клиентов с наибольшим числом визитов.</summary>
    [HttpGet("top-clients")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<TopClientDto>>> GetTopClients(CancellationToken cancellationToken)
    {
        return Ok(await analytics.GetTopClientsAsync(cancellationToken));
    }

    /// <summary>Автомобили на мойке в указанный момент.</summary>
    [HttpGet("vehicles-in-wash")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<VehicleDto>>> GetVehiclesInWash(
        [FromQuery] DateTimeOffset? at,
        CancellationToken cancellationToken)
    {
        return Ok(await analytics.GetVehiclesInWashAsync(at, cancellationToken));
    }

    /// <summary>Пять самых популярных услуг.</summary>
    [HttpGet("top-services")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<TopServiceDto>>> GetTopServices(CancellationToken cancellationToken)
    {
        return Ok(await analytics.GetTopServicesAsync(cancellationToken));
    }

    /// <summary>Конец последней записи в боксе.</summary>
    [HttpGet("boxes/{boxNumber:int}/availability")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BoxAvailabilityDto>> GetBoxAvailability(
        [Range(1, int.MaxValue)] int boxNumber,
        CancellationToken cancellationToken)
    {
        return Ok(await analytics.GetBoxAvailabilityAsync(boxNumber, cancellationToken));
    }

    /// <summary>Суммарная выручка по каждой услуге.</summary>
    [HttpGet("service-revenue")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ServiceRevenueDto>>> GetRevenue(CancellationToken cancellationToken)
    {
        return Ok(await analytics.GetRevenueAsync(cancellationToken));
    }
}
