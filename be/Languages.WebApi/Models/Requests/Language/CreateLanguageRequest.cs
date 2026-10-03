using FluentValidation;

namespace Languages.WebApi.Models.Requests.Language;

public sealed class CreateLanguageRequest
{
    public required string Name { get; set; }
}

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
    }
}
