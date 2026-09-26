using FluentValidation;

namespace Languages.WebApi.Models.LanguageDictionary.Requests.Update;

public class UpdateLanguageDictionaryRequestValidator : AbstractValidator<UpdateLanguageDictionaryRequest>
{
    public UpdateLanguageDictionaryRequestValidator()
    {
        When(d => d.Name.IsSpecified, () =>
        {
            RuleFor(d => d.Name.Value)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Name cannot be null or empty.")
                .MinimumLength(5)
                .WithMessage("Name should be at least 5 characters long.")
                .MaximumLength(20)
                .WithMessage("Name cannot exceed 20 characters.")
                .OverridePropertyName(nameof(UpdateLanguageDictionaryRequest.Name));
        });

        When(d => d.LevelID.IsSpecified, () =>
        {
            RuleFor(d => d.LevelID.Value)
                .GreaterThan(0)
                .WithMessage("Level ID must be a positive integer.")
                .OverridePropertyName(nameof(UpdateLanguageDictionaryRequest.LevelID));
        });
    }
}
