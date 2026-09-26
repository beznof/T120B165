using FluentValidation;

namespace Languages.WebApi.Models.LanguageLevel.Requests.Delete;

public class DeleteLanguageLevelRequestValidator : AbstractValidator<DeleteLanguageLevelRequest>
{
    public DeleteLanguageLevelRequestValidator()
    {
        RuleFor(l => l.Id)
            .GreaterThan(0)
            .WithMessage("ID must be a positive integer.");

        RuleFor(l => l.LanguageID)
            .GreaterThan(0)
            .WithMessage("Language ID must be a positive integer.");
    }
}
