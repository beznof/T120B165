using FluentValidation;

namespace Languages.WebApi.Models.Requests.LanguageDictionary;

public class LanguageDictionaryRequest : BaseLanguageDictionaryRequest
{
    public required int DictionaryId { get; set; }
}

public class LanguageDictionaryRequestValidator : AbstractValidator<LanguageDictionaryRequest>
{
    public LanguageDictionaryRequestValidator()
    {
        Include(new BaseLanguageDictionaryRequestValidator());
        
        RuleFor(r => r.DictionaryId)
            .GreaterThan(0)
            .WithMessage("Dictionary ID must be a positive integer.");
    }
}