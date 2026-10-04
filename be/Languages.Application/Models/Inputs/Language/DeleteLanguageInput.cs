using FluentValidation;

namespace Languages.Application.Models.Inputs.Language;

public sealed class DeleteLanguageInput : LanguageInput
{ }

public class DeleteLanguageInputValidator : AbstractValidator<DeleteLanguageInput>
{
    public DeleteLanguageInputValidator()
    {
        Include(new LanguageInputValidator());
    }
}
