using FluentValidation;

namespace Languages.WebApi.Models.Requests.LanguageDictionary;

public sealed class CreateLanguageDictionaryRequest
{
    public required string Name { get; set; }
}

public class CreateLanguageDictionaryRequestValidator : AbstractValidator<CreateLanguageDictionaryRequest>
{
    public CreateLanguageDictionaryRequestValidator()
    {
        RuleFor(d => d.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MinimumLength(5)
            .WithMessage("Name should be at least 5 characters long.")
            .MaximumLength(20)
            .WithMessage("Name cannot exceed 20 characters.");
    }
}

