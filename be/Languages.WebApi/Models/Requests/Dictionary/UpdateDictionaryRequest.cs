using Languages.Application.Models.Common;

namespace Languages.WebApi.Models.Requests.Dictionary;

public sealed class UpdateDictionaryRequest
{
    public Optional<string?> Name { get; set; }
}
