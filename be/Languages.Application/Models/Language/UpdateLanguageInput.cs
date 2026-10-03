using Languages.Application.Models.Common;

namespace Languages.Application.Models.Language;

public class UpdateLanguageInput
{
    public required Optional<string?> Name { get; init; }
}