using System.Text.Json;
using System.Text.Json.Serialization;
using Languages.WebApi.Models.Common;

namespace Languages.WebApi.Serialization;

public sealed class PatchFieldJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(PatchField<>);

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var valueType = typeToConvert.GetGenericArguments()[0];
        var converterType = typeof(PatchFieldJsonConverter<>).MakeGenericType(valueType);
        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }

    private sealed class PatchFieldJsonConverter<T> : JsonConverter<PatchField<T>>
    {
        public override bool HandleNull => true;

        public override PatchField<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            new(JsonSerializer.Deserialize<T>(ref reader, options)!);

        public override void Write(Utf8JsonWriter writer, PatchField<T> value, JsonSerializerOptions options)
        {
            if (!value.IsSpecified)
            {
                throw new JsonException(
                    "Omitted patch fields must be skipped with JsonIgnoreCondition.WhenWritingDefault.");
            }

            JsonSerializer.Serialize(writer, value.Value, options);
        }
    }
}
