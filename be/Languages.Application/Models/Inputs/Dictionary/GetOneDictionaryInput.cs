using FluentValidation;
using Languages.Application.Models.Inputs.Common;

namespace Languages.Application.Models.Inputs.Dictionary;

public class GetOneDictionaryInput : DictionaryInput
{ }

public class GetOneDictionaryInputValidator : AbstractValidator<GetOneDictionaryInput>
{
    public GetOneDictionaryInputValidator()
    {
        Include(new DictionaryInputValidator());
    }
}