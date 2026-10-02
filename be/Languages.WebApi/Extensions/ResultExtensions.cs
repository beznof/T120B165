using System.Net;
using Languages.Application.Common;
using Languages.WebApi.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace Languages.WebApi.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result, HttpStatusCode? statusCode = null) => 
        CreateActionResult(result: result, data: result.Data, statusCode: statusCode);
    
    public static IActionResult ToActionResult(this Result result, HttpStatusCode? statusCode = null) => 
        CreateActionResult(result: result, data: null, statusCode: statusCode);
    
    private static IActionResult CreateActionResult(Result result, object? data, HttpStatusCode? statusCode = null)
    {

        if (statusCode == null)
        {
            statusCode = result switch
            {
                { IsSuccessful: false, Error: ResultErrorKind.ValidationFailed } => HttpStatusCode.BadRequest,
                { IsSuccessful: false, Error: ResultErrorKind.AuthenticationFailed } => HttpStatusCode.Unauthorized,
                { IsSuccessful: false, Error: ResultErrorKind.AuthorizationFailed } => HttpStatusCode.Forbidden,
                { IsSuccessful: false, Error: ResultErrorKind.ResourceNotFound } => HttpStatusCode.NotFound,
                { IsSuccessful: false, Error: ResultErrorKind.ConflictOccured } => HttpStatusCode.Conflict,
                { IsSuccessful: true, Error: null } => HttpStatusCode.OK,
                _ => throw new InvalidOperationException("Ambiguous Result type"),
            };
        }

        var response = new ApiResponse(Message: result.Message, Data: data);
        
        return new ObjectResult(value: response)
        {
            StatusCode = (int)statusCode
        };
    }
}
