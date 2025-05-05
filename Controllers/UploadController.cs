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
    public async Task<IActionResult> Upload([FromForm] IFormFile file)
    {
        try
        {
            var url = await _cloudinary.UploadImageAsync(file);
            return Ok(new { url });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}