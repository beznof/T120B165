using Languages.Application.Models.Common;
using Languages.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Languages.WebApi.Filters;

public sealed class ApiExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var result = new Result
        (
            Message: "Server couldn't process the request.",
            IsSuccessful: false,
            Error: ResultErrorKind.ServerErrorOccured
        );
        
        context.Result = result.ToActionResult();
        
        context.ExceptionHandled = true;
    }
}
