using FluentValidation;
using Languages.WebApi.Models.Common;

namespace Languages.WebApi.Models.Language.Requests;

public class ReadManyLanguagesRequestValidator : AbstractValidator<ReadManyLanguagesRequest>
{
    public ReadManyLanguagesRequestValidator()
    {
        Include(new ReadManyRequestValidator());
    }
}
