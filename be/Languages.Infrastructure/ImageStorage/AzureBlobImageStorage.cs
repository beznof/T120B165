using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Languages.Application.Interfaces;
using Languages.Application.Models.Inputs.Common;

namespace Languages.Infrastructure.ImageStorage;

public class AzureBlobImageStorage(BlobContainerClient containerClient) : IImageStorage
{
    public async Task<string> UploadImageAsync(string location, SetImageInput image, CancellationToken cancellationToken)
    {
        var blob = containerClient.GetBlobClient(location);

        await blob.UploadAsync(
            image.ImageStream,
            new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = image.ContentType
                }
            },
            cancellationToken
        );
        
        return blob.Uri.AbsoluteUri;
    }
    
    public async Task DeleteImageAsync(string url, CancellationToken cancellationToken)
    {
        var uri = new BlobUriBuilder(new Uri(url));
        await containerClient.DeleteBlobIfExistsAsync(uri.BlobName, DeleteSnapshotsOption.IncludeSnapshots, cancellationToken: cancellationToken);
    }
}