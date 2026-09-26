using FluentValidation;
using Languages.WebApi.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Languages.WebApi.Filters;

public sealed class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        List<string> errors = [];
        
        foreach (var parameter in context.ActionDescriptor.Parameters)
        {
            if (!context.ActionArguments.TryGetValue(parameter.Name, out var argument) || argument is null)
                continue;

            var validatorType = typeof(IValidator<>).MakeGenericType(parameter.ParameterType);
            var validators = context.HttpContext.RequestServices.GetServices(validatorType).Cast<IValidator>();

            foreach (var validator in validators)
            {
                var result = await validator.ValidateAsync(new ValidationContext<object>(argument), context.HttpContext.RequestAborted);
                result.Errors.ForEach(e => errors.Add(e.ErrorMessage));
            }
        }

        if (errors.Count > 0)
        {
            context.Result = new BadRequestObjectResult(new ApiResponse
            (
                Success: false,
                Message: "Invalid request",
                UserFriendlyMessage: "One or more validation errors occurred.",
                Errors: errors
            ));
            return;
        }

        await next();
    }
}
