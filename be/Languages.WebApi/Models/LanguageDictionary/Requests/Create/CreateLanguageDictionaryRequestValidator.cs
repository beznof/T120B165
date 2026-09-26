using FluentValidation;
using Languages.WebApi.Models.Common;

namespace Languages.WebApi.Models.LanguageDictionary.Requests.Create;

public class CreateLanguageDictionaryRequestValidator : AbstractValidator<CreateLanguageDictionaryRequest>
{
    public CreateLanguageDictionaryRequestValidator()
    {
        RuleFor(d => d.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MinimumLength(5)
            .WithMessage("Name should be at least 5 characters long.")
            .MaximumLength(20)
            .WithMessage("Name cannot exceed 20 characters.");

        RuleFor(d => d.LanguageID)
            .GreaterThan(0)
            .WithMessage("Language ID must be a positive integer.");

        When(d => d.BackgroundImage != null, () =>
        {
            RuleFor(d => d.BackgroundImage!)
                .SetValidator(new ImageFileValidator());
        });
    }
}
