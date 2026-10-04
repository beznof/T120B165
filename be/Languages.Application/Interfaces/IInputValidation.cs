using FluentValidation.Results;

namespace Languages.Application.Interfaces;

public interface IInputValidation
{
    Task<ValidationResult> ValidateAsync<T>(T input, CancellationToken cancellationToken = default);
}
