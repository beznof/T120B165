using FluentValidation;

namespace Languages.Application.Models.Inputs.Entry;

public sealed class GetOneEntryInput : EntryInput
{ }

public class GetOneEntryInputValidator : AbstractValidator<GetOneEntryInput>
{
    public GetOneEntryInputValidator()
    {
        Include(new EntryInputValidator());
    }
}
