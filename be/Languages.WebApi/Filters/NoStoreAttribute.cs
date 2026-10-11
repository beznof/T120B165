using Microsoft.AspNetCore.Mvc.Filters;

namespace Languages.WebApi.Filters;

/// <summary>Prevents caching, including automatic model-binding error responses.</summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class NoStoreAttribute : Attribute, IResourceFilter
{
    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        // Resource filters run before model binding and the automatic 400 action filter.
        context.HttpContext.Response.Headers.CacheControl = "no-store";
    }

    public void OnResourceExecuted(ResourceExecutedContext context) { }
}
