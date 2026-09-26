using FluentValidation;

namespace Languages.WebApi.Models.Language.Requests.Update;

public class UpdateLanguageRequestValidator : AbstractValidator<UpdateLanguageRequest>
{
    public UpdateLanguageRequestValidator()
    {
        When(l => l.Name.IsSpecified, () =>
        {
            RuleFor(l => l.Name.Value)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Name cannot be null or empty.")
                .MaximumLength(100)
                .WithMessage("Name cannot exceed 100 characters.")
                .OverridePropertyName(nameof(UpdateLanguageRequest.Name));
        });

    }
}
