namespace Languages.Infrastructure.Options;

public sealed class AzureBlobStorageOptions
{
    public const string AppSettingsSectionName = "AzureBlobStorage";
    
    public string ContainerName { get; set; } = string.Empty;
}