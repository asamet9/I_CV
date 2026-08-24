using ICV.Application.DTOs.CvImport;
using ICV.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ICV.Api.Controllers
{
    [ApiController]
    [Route("api/cv-import")]
    [Authorize]
    public class CvImportController : ControllerBase
    {
        private readonly ICvImportService _cvImportService;

        public CvImportController(ICvImportService cvImportService)
        {
            _cvImportService = cvImportService;
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload(
            IFormFile file,
            [FromForm] int professionId,
            [FromForm] string title,
            CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("CV file cannot be empty.");
            }

            var userIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            await using var stream = file.OpenReadStream();

            var request = new ImportCvRequestDto
            {
                FileStream = stream,
                FileName = file.FileName,
                ContentType = file.ContentType,
                ProfessionId = professionId,
                Title = title
            };

            var result = await _cvImportService.ImportAsync(
                request,
                userId,
                cancellationToken);

            return Ok(result);
        }
    }
}