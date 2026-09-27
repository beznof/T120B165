using Languages.WebApi.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Languages.WebApi.Filters;

public sealed class ApiExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        context.Result = new ObjectResult(new ApiResponse
        (
            Message: "Server couldn't process the request."
        ))
        {
            StatusCode = 500
        };
        
        context.ExceptionHandled = true;
    }
}
