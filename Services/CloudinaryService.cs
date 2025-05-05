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

    public async Task<string> UploadImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new Exception("No file provided.");

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
            return result.SecureUrl.ToString();

        throw new Exception("Cloudinary upload failed.");
    }
}
