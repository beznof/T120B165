namespace Languages.WebApi.Models.Common;

/// <summary>The response returned by the API.</summary>
/// <param name="Message">The message describing the result of the request.</param>
public record ApiResponse(string Message)
{
    /// <summary>
    /// The links to related resources or available actions.
    /// Each key is a link relation and each value is its URL.\
    /// </summary>
    public IReadOnlyDictionary<string, string> Links { get; set; } = new Dictionary<string, string>();
};

/// <summary>The response returned by the API with data.</summary>
/// <typeparam name="T">The type of the response data.</typeparam>
/// <param name="Message">The message describing the result of the request.</param>
public sealed record ApiResponse<T>(string Message) : ApiResponse(Message)
{
    /// <summary>
    /// The data returned by the request.
    /// </summary>
    public T? Data { get; init; } = default;
}
