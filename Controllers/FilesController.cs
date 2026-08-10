using EmployeeManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    private readonly S3Service _s3Service;

    public FilesController(S3Service s3Service)
    {
        _s3Service = s3Service;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Please select a file.");
        }

        var fileName =
            await _s3Service.UploadFileAsync(file);

        return Ok(new
        {
            message = "File uploaded successfully.",
            fileName = fileName
        });
    }
}