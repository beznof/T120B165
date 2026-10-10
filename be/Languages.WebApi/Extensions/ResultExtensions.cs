using System.Net;
using Languages.Application.Models.Common;
using Languages.WebApi.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace Languages.WebApi.Extensions;

public static class ResultExtensions
{
    public static ActionResult ToActionResult<T>(this Result<T> result, HttpStatusCode? statusCode = null, IReadOnlyDictionary<string, string>? links = null)
    {
        ApiResponse response;

        if (result.Data != null)
        {
            response = new ApiResponse<T>(Message: result.Message)
            {
                Data = result.Data,
            };
        }
        else
        {
            response = new ApiResponse(Message: result.Message);
        }

        if (links != null)
        {
            response.Links = links;
        }
        
        var finalStatusCode = DetermineStatusCode(result, statusCode);
        
        return new ObjectResult(value: response is ApiResponse<T> ? (response as ApiResponse<T>) : response)
        {
            StatusCode = (int)finalStatusCode
        };
    }
    
    public static ActionResult ToActionResult(this Result result, HttpStatusCode? statusCode = null, IReadOnlyDictionary<string, string>? links = null) 
    {
        var response = new ApiResponse(Message: result.Message);
        
        if (links != null)
        {
            response.Links = links;
        }
        
        var finalStatusCode = DetermineStatusCode(result, statusCode);
        
        return new ObjectResult(value: response)
        {
            StatusCode = (int)finalStatusCode
        };
    }
    
    private static HttpStatusCode DetermineStatusCode(Result result, HttpStatusCode? statusCode = null)
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
                { IsSuccessful: false, Error: ResultErrorKind.ServerErrorOccured } => HttpStatusCode.InternalServerError,
                { IsSuccessful: true, Error: null } => HttpStatusCode.OK,
                _ => throw new InvalidOperationException("Ambiguous Result type"),
            };
        }

        return statusCode.Value;
    }
}
