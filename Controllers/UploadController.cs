using EcommerceApp.Models;
using EcommerceApp.Models.DTOs;
using EcommerceApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    private readonly CloudinaryService _cloudinary;

    public UploadController(CloudinaryService cloudinary)
    {
        _cloudinary = cloudinary;
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload([FromForm] FileUploadRequest request)
    {
        if (request.Files.Count == 0)
            return BadRequest(ApiResponse<List<string>>.Fail("No files provided."));

        try
        {
            var urls = await _cloudinary.UploadImagesAsync(request.Files);
            return Ok(ApiResponse<List<string>>.SuccessResponse(urls, "Files uploaded."));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<List<string>>.Fail(ex.Message));
        }
    }

}