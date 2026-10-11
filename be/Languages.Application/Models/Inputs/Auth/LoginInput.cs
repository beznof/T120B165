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
        RuleFor(i => i.Username)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithMessage("Username is required.")
            .Must(username => username.Trim().Length <= 50)
                .WithMessage("Username cannot exceed 50 characters.");

        RuleFor(i => i.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithMessage("Password is required.")
            .MaximumLength(64)
                .WithMessage("Password cannot exceed 64 characters.");
    }
}