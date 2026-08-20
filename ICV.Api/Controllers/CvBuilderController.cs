using ICV.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ICV.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CvBuilderController : ControllerBase
    {
        private readonly ICvBuilderService _cvBuilderService;

        public CvBuilderController(ICvBuilderService cvBuilderService)
        {
            _cvBuilderService = cvBuilderService;
        }

        [HttpPost("{cvId}")]
        public async Task<IActionResult> BuildCv(int cvId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            if (!int.TryParse(userIdClaim.Value, out var userId))
                return Unauthorized();

            await _cvBuilderService.BuildFromAnswersAsync(
                cvId,
                userId);

            return Ok(new
            {
                message = "CV cevaplardan başarıyla oluşturuldu."
            });
        }
    }
}