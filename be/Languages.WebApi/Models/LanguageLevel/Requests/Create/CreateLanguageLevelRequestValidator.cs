using FluentValidation;

namespace Languages.WebApi.Models.LanguageLevel.Requests.Create;

public class CreateLanguageLevelRequestValidator : AbstractValidator<CreateLanguageLevelRequest>
{
    public CreateLanguageLevelRequestValidator()
    {
        RuleFor(l => l.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(50)
            .WithMessage("Name cannot exceed 50 characters.");

        RuleFor(l => l.LanguageID)
            .GreaterThan(0)
            .WithMessage("Language ID must be a positive integer.");
    }
}
