using FluentValidation.Results;
using Languages.Application.Models.Common;

namespace Languages.Application.Extensions;

public static class ValidationResultExtensions
{
    public static Result ToFailureResult(this ValidationResult validationResult) => 
        Result.Failure(GetErrorMessage(validationResult), ResultErrorKind.ValidationFailed);
    
    public static Result<T> ToFailureResult<T>(this ValidationResult validationResult) => 
        Result.Failure(GetErrorMessage(validationResult), ResultErrorKind.ValidationFailed, default(T));

    private static string GetErrorMessage(ValidationResult validationResult)
    {
        return validationResult.Errors.FirstOrDefault()?.ErrorMessage ?? throw new NullReferenceException("Unknown validation error");
    }
}