using FluentValidation;

namespace Languages.Application.Models.Inputs.Common;

public class GetManyInput
{
    public required int Page { get; init; }
    public required int PageSize { get; init; }
    public string? Search { get; init; } = null;
}

public class GetManyInputValidator : AbstractValidator<GetManyInput>
{
    public GetManyInputValidator()
    {
        RuleFor(r => r.Page)
            .GreaterThan(0)
            .WithMessage("Page must be a positive integer.")
            .LessThanOrEqualTo(10000)
            .WithMessage("Page must can't be greater than 10000.");

        RuleFor(r => r.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100.");

        When(r => r.Search != null, () =>
        {
            RuleFor(r => r.Search)
                .MaximumLength(500)
                .WithMessage("Search cannot exceed 500 characters.");
        });
    }
}
