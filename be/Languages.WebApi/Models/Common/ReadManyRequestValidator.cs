using FluentValidation;

namespace Languages.WebApi.Models.Common;

public class ReadManyRequestValidator : AbstractValidator<ReadManyRequest>
{
    public ReadManyRequestValidator()
    {
        RuleFor(r => r.Page)
            .GreaterThan(0)
            .WithMessage("Page must be a positive integer.");

        RuleFor(r => r.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100.");

        RuleFor(r => r.Search)
            .MaximumLength(500)
            .WithMessage("Search cannot exceed 500 characters.");
    }
}
