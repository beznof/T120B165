namespace Languages.Application.Models.Common;

public sealed class SetImageInput
{
    public required Stream ImageStream { get; init; }
    public required string ContentType { get; init; }
    public required long Length { get; init; }
}
