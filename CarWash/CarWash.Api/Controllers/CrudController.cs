using CarWash.Contracts.Crud;
using Microsoft.AspNetCore.Mvc;

namespace CarWash.Api.Controllers;

/// <summary>HTTP-операции с коллекцией.</summary>
[ApiController]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
public abstract class CrudController<TDto, TEditDto>(ICrudService<TDto, TEditDto> service) : ControllerBase
    where TDto : class, IEntityDto
{
    /// <summary>Возвращает список записей.</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<TDto>>> List(CancellationToken cancellationToken)
    {
        return Ok(await service.ListAsync(cancellationToken));
    }

    /// <summary>Возвращает запись по номеру.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TDto>> Get(int id, CancellationToken cancellationToken)
    {
        var entity = await service.FindAsync(id, cancellationToken);
        return entity is null ? NotFound() : Ok(entity);
    }

    /// <summary>Добавляет запись.</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<TDto>> Create(TEditDto request, CancellationToken cancellationToken)
    {
        var entity = await service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
    }

    /// <summary>Обновляет запись по номеру.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<TDto>> Update(int id, TEditDto request, CancellationToken cancellationToken)
    {
        var entity = await service.UpdateAsync(id, request, cancellationToken);
        return entity is null ? NotFound() : Ok(entity);
    }

    /// <summary>Удаляет запись по номеру.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var removed = await service.DeleteAsync(id, cancellationToken);
        return removed ? NoContent() : NotFound();
    }
}
