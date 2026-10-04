using FluentValidation;

namespace Languages.Application.Models.Inputs.Entry;

public sealed class DeleteEntryInput : EntryInput
{ }

public class DeleteEntryInputValidator : AbstractValidator<DeleteEntryInput>
{
    public DeleteEntryInputValidator()
    {
        Include(new EntryInputValidator());
    }
}
