using FluentValidation;

namespace Languages.Application.Models.Inputs.Common;

public class AuthorInput
{
    public required int AuthorId { get; init; }
    public required bool IsAdmin { get; init; }
}

public class AuthorInputValidator : AbstractValidator<AuthorInput>
{
    public AuthorInputValidator()
    {
        RuleFor(x => x.AuthorId)
            .GreaterThan(0)
            .WithMessage("Author Id must be greater than 0");
    }
}