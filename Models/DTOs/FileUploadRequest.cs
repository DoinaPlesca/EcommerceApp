namespace EcommerceApp.Models.DTOs;

public class FileUploadRequest
{
    public List<IFormFile> Files { get; set; } = new();
}