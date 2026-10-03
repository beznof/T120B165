using FluentValidation;
using Languages.Application.Models.Common;

namespace Languages.WebApi.Models.Requests.LanguageDictionary;

public sealed class UpdateLanguageDictionaryRequest
{
    public Optional<string?> Name { get; set; }
}

public class UpdateLanguageDictionaryRequestValidator : AbstractValidator<UpdateLanguageDictionaryRequest>
{
    public UpdateLanguageDictionaryRequestValidator()
    {
        When(d => d.Name.IsProvided, () =>
        {
            RuleFor(d => d.Name.Value)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Name cannot be null or empty.")
                .MinimumLength(5)
                .WithMessage("Name should be at least 5 characters long.")
                .MaximumLength(20)
                .WithMessage("Name cannot exceed 20 characters.");
        });

    }
}