using System.Data;
using FluentValidation;
using Languages.WebApi.Models.Common;

namespace Languages.WebApi.Models.LanguageLevel.Requests.ReadMany;

public class ReadManyLanguageLevelsRequestValidator : AbstractValidator<ReadManyLanguageLevelsRequest>
{
    public ReadManyLanguageLevelsRequestValidator()
    {
        Include(new ReadManyRequestValidator());
        
        RuleFor(l => l.LanguageID)
            .GreaterThan(0)
            .WithMessage("Language ID must be a positive integer.");
    }
}
