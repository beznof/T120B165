using FluentValidation;

namespace Languages.WebApi.Models.LanguageFamily.Requests.Delete;

public class DeleteLanguageFamilyRequestValidator : AbstractValidator<DeleteLanguageFamilyRequest>
{
    public DeleteLanguageFamilyRequestValidator()
    {
        RuleFor(f => f.Id)
            .GreaterThan(0)
            .WithMessage("ID must be a positive integer.");
    }
}
