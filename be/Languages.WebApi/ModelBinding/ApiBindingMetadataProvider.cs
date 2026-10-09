using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace Languages.WebApi.ModelBinding;

internal sealed class ApiBindingMetadataProvider : IBindingMetadataProvider
{
    public void CreateBindingMetadata(BindingMetadataProviderContext context)
    {
        var field = context.BindingMetadata.BinderModelName ?? context.Key.Name;
        var invalidValueMessage = field == null
            ? "Invalid value was provided."
            : $"Invalid value was provided for '{field}'.";
        var requiredValueMessage = field == null
            ? "Value is required."
            : $"'{field}' is required.";

        var bindingMessageProvider = context.BindingMetadata.ModelBindingMessageProvider!;

        bindingMessageProvider.SetAttemptedValueIsInvalidAccessor(
            (_, _) => invalidValueMessage
        );

        bindingMessageProvider.SetUnknownValueIsInvalidAccessor(
            _ => invalidValueMessage
        );

        bindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor(
            _ => invalidValueMessage
        );

        bindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(
            () => invalidValueMessage
        );

        bindingMessageProvider.SetValueIsInvalidAccessor(
            _ => invalidValueMessage
        );

        bindingMessageProvider.SetValueMustNotBeNullAccessor(
            _ => requiredValueMessage
        );

        bindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(
            () => "Request body is required."
        );
    }
}
