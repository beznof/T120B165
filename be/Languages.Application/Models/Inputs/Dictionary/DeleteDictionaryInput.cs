using FluentValidation;

namespace Languages.Application.Models.Inputs.Dictionary;

public sealed class DeleteDictionaryInput : DictionaryInput
{ }

public class DeleteDictionaryInputValidator : AbstractValidator<DeleteDictionaryInput>
{
    public DeleteDictionaryInputValidator()
    {
        Include(new DictionaryInputValidator());
    }
}
