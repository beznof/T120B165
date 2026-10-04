namespace Languages.Application.Models.Common;

public readonly struct Optional<T>
{
    public bool IsProvided { get; } = false;
    public T? Value { get; } = default;

    public Optional(T? value)
    {
        IsProvided = true;
        Value = value;
    }
}
