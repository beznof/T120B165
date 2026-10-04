using FluentValidation;
using FluentValidation.Results;
using Languages.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Languages.Infrastructure.Validation;

public sealed class InputValidation(IServiceProvider serviceProvider) : IInputValidation
{
    public Task<ValidationResult> ValidateAsync<T>(T input, CancellationToken cancellationToken = default)
    {
        var validator = serviceProvider.GetRequiredService<IValidator<T>>();
        return validator.ValidateAsync(input, cancellationToken);
    }
}
