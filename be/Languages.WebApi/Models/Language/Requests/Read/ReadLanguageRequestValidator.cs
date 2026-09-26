using FluentValidation;

namespace Languages.WebApi.Models.Language.Requests.Read;

public class ReadLanguageRequestValidator : AbstractValidator<ReadLanguageRequest>
{
    public ReadLanguageRequestValidator()
    {
        RuleFor(l => l.Id)
            .GreaterThan(0)
            .WithMessage("ID must be a positive integer.");
    }
}
