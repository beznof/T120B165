using Languages.Application.Models.Common;

namespace Languages.WebApi.Models.Requests.LanguageDictionary;

public sealed class UpdateLanguageDictionaryRequest
{
    public Optional<string?> Name { get; set; }
}
