using FluentValidation;
using Languages.Application.Models.Common;

namespace Languages.WebApi.Models.Requests.Language;

public sealed class UpdateLanguageRequest
{
    public Optional<string?> Name { get; set; }
}

public class UpdateLanguageRequestValidator : AbstractValidator<UpdateLanguageRequest>
{
    public UpdateLanguageRequestValidator()
    {
        When(l => l.Name.IsProvided, () =>
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
