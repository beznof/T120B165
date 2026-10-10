using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace Languages.WebApi.OpenApi;

/// <summary>
/// Marks a bound property as required in OpenAPI without changing binding or validation.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = true)]
public sealed class OpenApiRequiredAttribute : Attribute
{ }

internal sealed class RequiredParameterMetadataProvider : IApiDescriptionProvider
{
    public int Order => 0;

    public void OnProvidersExecuting(ApiDescriptionProviderContext context)
    { }

    public void OnProvidersExecuted(ApiDescriptionProviderContext context)
    {
        foreach (var description in context.Results)
        {
            foreach (var parameter in description.ParameterDescriptions)
            {
                if (parameter.ModelMetadata is DefaultModelMetadata metadata
                    && metadata.Attributes.PropertyAttributes?.OfType<OpenApiRequiredAttribute>().Any() == true)
                    parameter.IsRequired = true;
            }
        }
    }
}
