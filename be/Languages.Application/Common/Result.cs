namespace Languages.Application.Common;

public record class Result(string Message, bool IsSuccessful = true, ResultErrorKind? Error = null);

public sealed record class Result<T>(string Message, T? Data = default, bool IsSuccessful = true, ResultErrorKind? Error = null) : Result(Message, IsSuccessful, Error);
