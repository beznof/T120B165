using FluentValidation;

namespace Languages.Application.Models.Inputs.Language;

public sealed class GetOneLanguageInput : LanguageInput
{ }

public class GetOneLanguageInputValidator : AbstractValidator<GetOneLanguageInput>
{
    public GetOneLanguageInputValidator()
    {
        Include(new LanguageInputValidator());
    }
}
