namespace Languages.WebApi.Models.Common;

public record ApiResponse(string Message);
public sealed record ApiResponse<T>(string Message, T? Data = default) : ApiResponse(Message);
