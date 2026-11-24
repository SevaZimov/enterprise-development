using System.ComponentModel.DataAnnotations;
using CarWash.Application;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CarWash.Api;

internal sealed class ApiExceptionHandler(ILogger<ApiExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is ValidationException validation)
        {
            logger.LogWarning("Запрос отклонён: {Reason}", validation.Message);
            var problem = new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                ["request"] = [validation.Message]
            })
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Неверные данные запроса."
            };
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(problem, options: null, "application/problem+json", cancellationToken);
            return true;
        }

        var (status, title) = exception switch
        {
            ResourceConflictException conflict => (StatusCodes.Status409Conflict, conflict.Message),
            _ => (StatusCodes.Status500InternalServerError, "Не удалось обработать запрос.")
        };

        if (status == StatusCodes.Status500InternalServerError)
        {
            logger.LogError(exception, "Ошибка запроса {Method} {Path}", context.Request.Method, context.Request.Path);
        }
        else
        {
            logger.LogWarning("Запрос отклонён: {Reason}", exception.Message);
        }

        context.Response.StatusCode = status;
        var response = new ProblemDetails { Status = status, Title = title };
        await context.Response.WriteAsJsonAsync(response, options: null, "application/problem+json", cancellationToken);
        return true;
    }
}
