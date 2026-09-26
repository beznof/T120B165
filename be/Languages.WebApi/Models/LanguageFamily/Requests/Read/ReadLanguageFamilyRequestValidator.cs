using FluentValidation;

namespace Languages.WebApi.Models.LanguageFamily.Requests.Read;

public class ReadLanguageFamilyRequestValidator : AbstractValidator<ReadLanguageFamilyRequest>
{
    public ReadLanguageFamilyRequestValidator()
    {
        RuleFor(f => f.Id)
            .GreaterThan(0)
            .WithMessage("ID must be a positive integer.");
    }
}
