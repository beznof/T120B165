using FluentValidation;

namespace Languages.Application.Models.Inputs.Auth;

public sealed class LoginInput
{
    public required string Username { get; init; }
    public required string Password { get; init; }
}

public sealed class LoginInputValidator : AbstractValidator<LoginInput>
{
    public LoginInputValidator()
    {
        RuleFor(i => i.Username.Trim())
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithMessage("Username is required.");

        RuleFor(i => i.Password.Trim())
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithMessage("Password is required.");
    }
}