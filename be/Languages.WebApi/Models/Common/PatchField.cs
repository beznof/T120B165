using System.Text.Json.Serialization;
using Languages.WebApi.Serialization;

namespace Languages.WebApi.Models.Common;

[JsonConverter(typeof(PatchFieldJsonConverterFactory))]
public readonly struct PatchField<T>
{
    public bool IsSpecified { get; } = false;
    public T Value { get; } = default;

    public PatchField(T value)
    {
        IsSpecified = true;
        Value = value;
    }
}
