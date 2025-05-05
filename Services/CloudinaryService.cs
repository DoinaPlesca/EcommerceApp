using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EcommerceApp.Configuration;
using Microsoft.Extensions.Options;

namespace EcommerceApp.Services;

public class CloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IOptions<CloudinarySettings> config)
    {
        var settings = config.Value;
        var account = new Account(settings.CloudName, settings.ApiKey, settings.ApiSecret);
        _cloudinary = new Cloudinary(account);
    }

    public async Task<List<string>> UploadImagesAsync(List<IFormFile> files)
    {
        var urls = new List<string>();

        foreach (var file in files)
        {
            if (file.Length == 0) continue;

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Folder = "listings",
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = false
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                urls.Add(result.SecureUrl.ToString());
            }
            else
            {
                throw new Exception($"Failed to upload file: {file.FileName}");
            }
        }

        return urls;
    }
}