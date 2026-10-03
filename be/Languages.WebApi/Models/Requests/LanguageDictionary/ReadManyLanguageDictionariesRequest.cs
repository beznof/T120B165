using FluentValidation;
using Languages.WebApi.Models.Common;

namespace Languages.WebApi.Models.Requests.LanguageDictionary;

public sealed class ReadManyLanguageDictionariesRequest : ReadManyRequest
{ }

public class ReadManyLanguageDictionariesRequestValidator : AbstractValidator<ReadManyLanguageDictionariesRequest>
{
    public ReadManyLanguageDictionariesRequestValidator()
    {
        Include(new ReadManyRequestValidator());
    }
}