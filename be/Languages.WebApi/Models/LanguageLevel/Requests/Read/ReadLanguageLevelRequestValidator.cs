using FluentValidation;

namespace Languages.WebApi.Models.LanguageLevel.Requests.Read;

public class ReadLanguageLevelRequestValidator : AbstractValidator<ReadLanguageLevelRequest>
{
    public ReadLanguageLevelRequestValidator()
    {
        RuleFor(l => l.Id)
            .GreaterThan(0)
            .WithMessage("ID must be a positive integer.");

        RuleFor(l => l.LanguageID)
            .GreaterThan(0)
            .WithMessage("Language ID must be a positive integer.");
    }
}
