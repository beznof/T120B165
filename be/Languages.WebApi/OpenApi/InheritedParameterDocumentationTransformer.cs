using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Languages.WebApi.OpenApi;

internal sealed class InheritedParameterDocumentationTransformer : IOpenApiOperationTransformer
{
    private static readonly ConcurrentDictionary<Assembly, Lazy<IReadOnlyDictionary<string, XElement>>> Documentation = new();

    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        if (operation.Parameters is null)
            return Task.CompletedTask;

        foreach (var parameterDescription in context.Description.ParameterDescriptions)
        {
            var location = parameterDescription.Source == BindingSource.Query ? ParameterLocation.Query
                : parameterDescription.Source == BindingSource.Path ? ParameterLocation.Path
                : parameterDescription.Source == BindingSource.Header ? ParameterLocation.Header
                : (ParameterLocation?)null;
            var metadata = parameterDescription.ModelMetadata;
            if (location is null || metadata?.MetadataKind != ModelMetadataKind.Property
                || metadata.ContainerType is not { } containerType || metadata.PropertyName is not { } propertyName)
                continue;

            var property = containerType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (property?.DeclaringType is not { } declaringType || declaringType == containerType)
                continue;

            var parameter = operation.Parameters.FirstOrDefault(p => p.In == location
                && string.Equals(p.Name, parameterDescription.Name, StringComparison.OrdinalIgnoreCase));
            OpenApiParameter? target = parameter switch
            {
                OpenApiParameterReference { Target: OpenApiParameter referenced } => referenced,
                OpenApiParameter direct => direct,
                _ => null
            };
            if (target is null)
                continue;

            // XML comments belong to the property-declaring base type, not the derived request type.
            var documentationType = declaringType.IsGenericType ? declaringType.GetGenericTypeDefinition() : declaringType;
            var memberId = $"P:{documentationType.FullName!.Replace('+', '.')}.{property.Name}";
            var documentation = Documentation.GetOrAdd(declaringType.Assembly,
                assembly => new Lazy<IReadOnlyDictionary<string, XElement>>(() => LoadDocumentation(assembly))).Value;
            if (!documentation.TryGetValue(memberId, out var member))
                continue;

            // Preserve descriptions/examples already supplied by endpoint metadata or other transformers.
            if (string.IsNullOrWhiteSpace(target.Description))
            {
                var description = string.Join("\n\n", new[] { "summary", "value", "remarks" }
                    .Select(name => member.Element(name)?.Value)
                    .Where(value => !string.IsNullOrWhiteSpace(value))
                    .Select(value => Regex.Replace(value!.Trim(), @"\s+", " ")));
                if (description.Length > 0)
                    target.Description = description;
            }

            if (member.Element("deprecated") is not null)
                target.Deprecated = true;

            if (target.Example is null && target.Examples is not { Count: > 0 }
                && member.Element("example") is { } example)
            {
                try
                {
                    target.Example = JsonNode.Parse(example.Value.Trim());
                }
                catch (JsonException)
                {
                    target.Example = JsonValue.Create(example.Value.Trim());
                }
            }
        }

        return Task.CompletedTask;
    }

    private static IReadOnlyDictionary<string, XElement> LoadDocumentation(Assembly assembly)
    {
        var path = Path.ChangeExtension(assembly.Location, ".xml");
        if (!File.Exists(path))
            return new Dictionary<string, XElement>();

        return XDocument.Load(path).Descendants("member")
            .Where(member => member.Attribute("name") is not null)
            .ToDictionary(member => member.Attribute("name")!.Value, member => member, StringComparer.Ordinal);
    }
}
