using FluentValidation;
using Languages.WebApi.Models.Common;

namespace Languages.WebApi.Models.LanguageFamily.Requests.ReadMany;

public class ReadManyLanguageFamiliesRequestValidator : AbstractValidator<ReadManyLanguageFamiliesRequest>
{
    public ReadManyLanguageFamiliesRequestValidator()
    {
        Include(new ReadManyRequestValidator());
    }
}
