using FluentValidation;

namespace Languages.WebApi.Models.LanguageFamily.Requests.Create;

public class CreateLanguageFamilyRequestValidator : AbstractValidator<CreateLanguageFamilyRequest>
{
    public CreateLanguageFamilyRequestValidator()
    {
        RuleFor(f => f.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(100)
            .WithMessage("Name cannot exceed 100 characters.");
    }
}
