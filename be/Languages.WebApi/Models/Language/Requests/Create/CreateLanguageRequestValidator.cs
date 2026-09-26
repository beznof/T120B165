using FluentValidation;
using Languages.WebApi.Models.Common;

namespace Languages.WebApi.Models.Language.Requests.Create;

public class CreateLanguageRequestValidator : AbstractValidator<CreateLanguageRequest>
{
    public CreateLanguageRequestValidator()
    {
        RuleFor(l => l.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(100)
            .WithMessage("Name cannot exceed 100 characters.");

        When(l => l.LanguageFamilyID != null, () =>
        {
            RuleFor(l => l.LanguageFamilyID)
                .GreaterThan(0)
                .WithMessage("LanguageFamily ID must be a positive integer.");
        });

        When(l => l.BackgroundImage != null, () =>
        {
            RuleFor(l => l.BackgroundImage!)
                .SetValidator(new ImageFileValidator());
        });
    }
}
