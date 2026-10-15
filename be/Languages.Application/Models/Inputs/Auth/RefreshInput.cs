using FluentValidation;

namespace Languages.Application.Models.Inputs.Auth;

public sealed class RefreshInput
{
    public required string RefreshToken { get; init; }
}

public sealed class RefreshInputValidator : AbstractValidator<RefreshInput>
{
    public RefreshInputValidator()
    {
        RuleFor(i => i.RefreshToken.Trim())
            .NotEmpty()
                .WithMessage("Refresh token is required.");
    }
}