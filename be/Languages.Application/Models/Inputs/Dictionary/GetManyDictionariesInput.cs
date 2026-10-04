using FluentValidation;
using Languages.Application.Models.Inputs.Common;

namespace Languages.Application.Models.Inputs.Dictionary;

public class GetManyDictionariesInput : BaseDictionaryInput
{
    public required GetManyInput Pagination { get; init; }
}

public class GetManyDictionariesInputValidator : AbstractValidator<GetManyDictionariesInput>
{
    public GetManyDictionariesInputValidator()
    {
        Include(new BaseDictionaryInputValidator());

        RuleFor(l => l.Pagination)
            .NotNull()
            .SetValidator(new GetManyInputValidator());
    }
}
