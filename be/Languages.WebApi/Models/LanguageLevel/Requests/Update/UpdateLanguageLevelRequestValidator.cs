using FluentValidation;

namespace Languages.WebApi.Models.LanguageLevel.Requests.Update;

public class UpdateLanguageLevelRequestValidator : AbstractValidator<UpdateLanguageLevelRequest>
{
    public UpdateLanguageLevelRequestValidator()
    {
        When(l => l.Name.IsSpecified, () =>
        {
            RuleFor(l => l.Name.Value)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Name cannot be null or empty.")
                .MaximumLength(50)
                .WithMessage("Name cannot exceed 50 characters.")
                .OverridePropertyName(nameof(UpdateLanguageLevelRequest.Name));
        });
    }
}
