using FluentValidation;
using Languages.Application.Common;
using Languages.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Languages.WebApi.Filters;

public sealed class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        string? errorMessage = null;
        bool errorEncountered = false;
        
        foreach (var parameter in context.ActionDescriptor.Parameters)
        {
            if (!context.ActionArguments.TryGetValue(parameter.Name, out var argument) || argument is null)
                continue;

            var validatorType = typeof(IValidator<>).MakeGenericType(parameter.ParameterType);
            var validators = context.HttpContext.RequestServices.GetServices(validatorType).Cast<IValidator>();

            foreach (var validator in validators)
            {
                var result = await validator.ValidateAsync(new ValidationContext<object>(argument), context.HttpContext.RequestAborted);

                if (!result.IsValid)
                {
                    errorEncountered = true;
                    errorMessage = result.Errors.FirstOrDefault()?.ErrorMessage;
                    break;
                }
            }

            if (errorEncountered)
            {
                break;
            }
        }

        if (errorEncountered)
        {
            var result = new Result
            (
                Message: errorMessage ?? "Request failed.",
                IsSuccessful: false,
                Error: ResultErrorKind.ValidationFailed
            );

            context.Result = result.ToActionResult();
            
            return;
        }

        await next();
    }
}
