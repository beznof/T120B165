using Languages.Application.Models.Common;

namespace Languages.WebApi.Models.Requests.Language;

public sealed class UpdateLanguageRequest
{
    public Optional<string?> Name { get; set; }
}
