namespace Languages.Infrastructure.Options;

public sealed class AzureBlobStorageOptions
{
    public const string AppSettingsSectionName = "AzureBlobStorage";

    public string ServiceUri { get; set; } = string.Empty;
    public string ContainerName { get; set; } = string.Empty;
}