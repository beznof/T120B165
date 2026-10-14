using FluentValidation;
using Languages.Domain.Enums;

namespace Languages.Application.Models.Inputs.Auth;

public sealed class RegisterInput
{
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required UserRole Role { get; init; }
}

public sealed class RegisterInputValidator : AbstractValidator<RegisterInput>
{
    public RegisterInputValidator()
    {
        RuleFor(i => i.Username.Trim())
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithMessage("Username is required.")
            .MinimumLength(5)
                .WithMessage("Username must contain at least 5 characters.")
            .Must(username => username.Trim().Length <= 50)
                .WithMessage("Username cannot exceed 50 characters.");

        RuleFor(i => i.Password.Trim())
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithMessage("Password is required.")
            .MinimumLength(8)
                .WithMessage("Password must contain at least 8 characters.")
            .MaximumLength(64)
                .WithMessage("Password cannot exceed 64 characters.");

        RuleFor(i => i.Role)
            .Must(role => role is UserRole.Learner or UserRole.Teacher)
                .WithMessage("Role must be Learner or Teacher.");
    }
}