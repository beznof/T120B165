using FluentValidation;
using Languages.Application.Models.Inputs.Common;

namespace Languages.Application.Models.Inputs.Language;

public sealed class GetManyLanguagesInput
{
    public required GetManyInput Pagination { get; init; }
}

public class GetManyLanguagesInputValidator : AbstractValidator<GetManyLanguagesInput>
{
    public GetManyLanguagesInputValidator()
    {
        RuleFor(l => l.Pagination)
            .NotNull()
            .SetValidator(new GetManyInputValidator());
    }
}
