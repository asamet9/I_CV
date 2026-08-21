using ICV.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ICV.Api.Controllers
{
    [ApiController]
    [Route("api/cvs/{cvId}/file")]
    [Authorize]
    public class CvFilesController : ControllerBase
    {
        private readonly ICvFileService _cvFileService;

        public CvFilesController(
            ICvFileService cvFileService)
        {
            _cvFileService = cvFileService;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(
            int cvId,
            IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Dosya seçilmedi.");
            }

            var userIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            if (file.ContentType != "application/pdf")
            {
                return BadRequest(
                    "Sadece PDF dosyaları yüklenebilir.");
            }

            var result = await _cvFileService.UploadAsync(
                cvId,
                file.OpenReadStream(),
                file.FileName,
                file.ContentType,
                file.Length,
                userId);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Get(int cvId)
        {
            var userIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            var result =
                await _cvFileService.GetByCvIdAsync(
                    cvId,
                    userId);

            if (result == null)
            {
                return NotFound(
                    "Bu CV'ye ait dosya bulunamadı.");
            }

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int cvId)
        {
            var userIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            var deleted =
                await _cvFileService.DeleteAsync(
                    cvId,
                    userId);

            if (!deleted)
            {
                return NotFound(
                    "Bu CV'ye ait dosya bulunamadı.");
            }

            return NoContent();
        }
    }
}