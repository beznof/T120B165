using FluentValidation;

namespace Languages.WebApi.Models.LanguageFamily.Requests.Update;

public class UpdateLanguageFamilyRequestValidator : AbstractValidator<UpdateLanguageFamilyRequest>
{
    public UpdateLanguageFamilyRequestValidator()
    {
        When(f => f.Name.IsSpecified, () =>
        {
            RuleFor(f => f.Name.Value)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Name cannot be null or empty.")
                .MaximumLength(100)
                .WithMessage("Name cannot exceed 100 characters.")
                .OverridePropertyName(nameof(UpdateLanguageFamilyRequest.Name));
        });
    }
}
