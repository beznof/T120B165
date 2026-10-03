using FluentValidation;

namespace Languages.WebApi.Models.Requests;

public class ReadManyRequest
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? Search { get; set; }
}

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