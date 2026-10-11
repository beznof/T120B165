using FluentValidation;

namespace Languages.Application.Models.Inputs.Auth;

public sealed class LogoutInput
{
    public required string RefreshToken { get; init; }
}

public sealed class LogoutInputValidator : AbstractValidator<LogoutInput>
{
    public LogoutInputValidator()
    {
        RuleFor(i => i.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required.");
    }
}
