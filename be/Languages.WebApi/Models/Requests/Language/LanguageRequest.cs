using FluentValidation;

namespace Languages.WebApi.Models.Requests.Language;

public class LanguageRequest
{
    public required int LanguageId { get; set; }
}

public class LanguageRequestValidator : AbstractValidator<LanguageRequest>
{
    public LanguageRequestValidator()
    {
        RuleFor(e => e.LanguageId)
            .GreaterThan(0)
            .WithMessage("Language ID must be a positive integer.");
    }
}