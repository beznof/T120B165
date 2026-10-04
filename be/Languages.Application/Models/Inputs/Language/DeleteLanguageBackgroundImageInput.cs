using FluentValidation;

namespace Languages.Application.Models.Inputs.Language;

public sealed class DeleteLanguageBackgroundImageInput : LanguageInput
{ }

public class DeleteLanguageBackgroundImageInputValidator : AbstractValidator<DeleteLanguageBackgroundImageInput>
{
    public DeleteLanguageBackgroundImageInputValidator()
    {
        Include(new LanguageInputValidator());
    }
}
