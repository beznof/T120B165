using FluentValidation;

namespace Languages.Application.Models.Inputs.Dictionary;

public sealed class DeleteDictionaryBackgroundImageInput : DictionaryInput
{ }

public class DeleteDictionaryBackgroundImageInputValidator : AbstractValidator<DeleteDictionaryBackgroundImageInput>
{
    public DeleteDictionaryBackgroundImageInputValidator()
    {
        Include(new DictionaryInputValidator());
    }
}
