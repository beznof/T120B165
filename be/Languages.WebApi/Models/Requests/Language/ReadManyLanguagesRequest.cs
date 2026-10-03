using FluentValidation;
using Languages.WebApi.Models.Common;

namespace Languages.WebApi.Models.Requests.Language;

public sealed class ReadManyLanguagesRequest : ReadManyRequest
{ }

public class ReadManyLanguagesRequestValidator : AbstractValidator<ReadManyLanguagesRequest>
{
    public ReadManyLanguagesRequestValidator()
    {
        Include(new ReadManyRequestValidator());
    }
}
