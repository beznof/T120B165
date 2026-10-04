using Languages.Application.Models.Inputs.Common;

namespace Languages.Application.Interfaces;

public interface IImageStorage
{
    Task<string> UploadImageAsync(string location, SetImageInput image, CancellationToken cancellationToken);
    Task DeleteImageAsync(string url, CancellationToken cancellationToken);
}