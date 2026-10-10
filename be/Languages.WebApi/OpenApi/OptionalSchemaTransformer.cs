using System.Reflection;
using System.Text.Json.Nodes;
using Languages.Application.Models.Common;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Languages.WebApi.OpenApi;

internal sealed class OptionalSchemaTransformer : IOpenApiSchemaTransformer
{
    public async Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        if (schema.Properties is not { } properties)
            return;

        foreach (var property in context.JsonTypeInfo.Properties)
        {
            var propertyType = property.PropertyType;
            if (!propertyType.IsGenericType || propertyType.GetGenericTypeDefinition() != typeof(Optional<>)
                || !properties.TryGetValue(property.Name, out var optionalSchema))
                continue;

            var valueType = propertyType.GetGenericArguments()[0];
            var underlyingType = Nullable.GetUnderlyingType(valueType) ?? valueType;
            var valueSchema = await context.GetOrCreateSchemaAsync(underlyingType, cancellationToken: cancellationToken);

            // Nullable reference annotations are on the property's generic argument, not the CLR Type.
            var isNullable = underlyingType != valueType
                || (!valueType.IsValueType && property.AttributeProvider is PropertyInfo propertyInfo
                    && new NullabilityInfoContext().Create(propertyInfo).GenericTypeArguments[0].ReadState == NullabilityState.Nullable);
            if (isNullable)
            {
                var isComponent = valueSchema.Metadata?.TryGetValue("x-schema-id", out var schemaId) == true
                    && schemaId is string { Length: > 0 };
                if (valueSchema.Type is { } type && !isComponent)
                    valueSchema.Type = type | JsonSchemaType.Null;
                else
                    valueSchema = new OpenApiSchema
                    {
                        AnyOf = [valueSchema, new OpenApiSchema { Type = JsonSchemaType.Null }]
                    };
            }

            var description = optionalSchema.Description;
            var example = optionalSchema.Example;
            // ASP.NET Core's XML transformer stores property annotations here until references are resolved.
            if (optionalSchema is OpenApiSchema { Metadata: { } metadata })
            {
                if (metadata.TryGetValue("x-ref-description", out var referenceDescription))
                    description = referenceDescription as string ?? description;
                if (metadata.TryGetValue("x-ref-example", out var referenceExample))
                    example = referenceExample as JsonNode ?? example;
            }

            valueSchema.Description = description ?? valueSchema.Description;
            valueSchema.Example = example ?? valueSchema.Example;
            valueSchema.Examples = optionalSchema.Examples ?? valueSchema.Examples;

            // Keep property annotations on the reference too when the underlying type is a component.
            if (valueSchema.Metadata is { } valueMetadata)
            {
                valueSchema.Metadata = new Dictionary<string, object>(valueMetadata);
                if (description is not null)
                    valueSchema.Metadata["x-ref-description"] = description;
                if (example is not null)
                    valueSchema.Metadata["x-ref-example"] = example;
            }

            properties[property.Name] = valueSchema;
            schema.Required?.Remove(property.Name);
        }
    }
}
