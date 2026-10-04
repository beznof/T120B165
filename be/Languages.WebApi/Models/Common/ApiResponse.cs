using System.Text.Json.Serialization;

namespace Languages.WebApi.Models.Common;

public record ApiResponse(string Message)
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyDictionary<string, string>? Links { get; init; } = null;
};

public sealed record ApiResponse<T>(string Message) : ApiResponse(Message)
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T? Data { get; init; } = default;
}
