using FluentValidation;

namespace Languages.WebApi.Models.Language.Requests.Delete;

public class DeleteLanguageRequestValidator : AbstractValidator<DeleteLanguageRequest>
{
    public DeleteLanguageRequestValidator()
    {
        RuleFor(l => l.Id)
            .GreaterThan(0)
            .WithMessage("ID must be a positive integer.");
    }
}
