using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace Languages.WebApi.Filters;

public sealed class ApiExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var (status, title, detail) = context.Exception switch
        {
            KeyNotFoundException exception => (404, "Resource not found", exception.Message),
            ValidationException exception => (400, "Invalid request", exception.Message),
            DbUpdateConcurrencyException => (409, "Conflict", "The resource changed or was deleted. Reload it and retry."),
            DbUpdateException => (409, "Conflict", "The change conflicts with existing data or a related resource."),
            _ => (0, string.Empty, string.Empty)
        };
        
        if (status == 0)
            return;

        context.Result = new ObjectResult(new ProblemDetails
        {
            Status = status, Title = title, Detail = detail, Instance = context.HttpContext.Request.Path
        }) { StatusCode = status };
        context.ExceptionHandled = true;
    }
}
