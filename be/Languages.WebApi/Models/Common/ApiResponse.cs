using System.Text.Json.Serialization;

namespace Languages.WebApi.Models.Common;

public record ApiResponse(string Message)
{
    public IReadOnlyDictionary<string, string>? Links { get; init; } = null;
};

public sealed record ApiResponse<T>(string Message) : ApiResponse(Message)
{
    public T? Data { get; init; } = default;
}
