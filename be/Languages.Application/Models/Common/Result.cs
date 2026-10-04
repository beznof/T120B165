namespace Languages.Application.Models.Common;

public record class Result(string Message, bool IsSuccessful = true, ResultErrorKind? Error = null)
{
    public static Result Success(string message) => new Result(message);
    public static Result<T> Success<T>(string message, T? data = default) => new Result<T>(message, data);
    public static Result Failure(string message, ResultErrorKind errorKind) => new Result(message, false, errorKind);
    public static Result<T> Failure<T>(string message, ResultErrorKind errorKind, T? data = default) => new Result<T>(message, data, false, errorKind);
}

public sealed record class Result<T>(string Message, T? Data = default, bool IsSuccessful = true, ResultErrorKind? Error = null) : Result(Message, IsSuccessful, Error);
