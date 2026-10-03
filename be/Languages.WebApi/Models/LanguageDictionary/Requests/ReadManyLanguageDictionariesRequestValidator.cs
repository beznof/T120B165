using FluentValidation;
using Languages.WebApi.Models.Common;

namespace Languages.WebApi.Models.LanguageDictionary.Requests;

public class ReadManyLanguageDictionariesRequestValidator : AbstractValidator<ReadManyLanguageDictionariesRequest>
{
    public ReadManyLanguageDictionariesRequestValidator()
    {
        Include(new ReadManyRequestValidator());
    }
}
