using ICV.Application.DTOs.CvAnalysis;
using ICV.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ICV.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CvAnalysisController : ControllerBase
    {
        private readonly ICvAnalysisService _cvAnalysisService;

        public CvAnalysisController(
            ICvAnalysisService cvAnalysisService)
        {
            _cvAnalysisService = cvAnalysisService;
        }

        [HttpPost("{cvId}/analyze")]
        public async Task<ActionResult<CvAnalysisResponseDto>> Analyze(
            int cvId,
            [FromBody] AnalyzeCvRequestDto request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null ||
                !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized();
            }

            var result = await _cvAnalysisService.AnalyzeAsync(
                cvId,
                request,
                userId);

            return Ok(result);
        }
    }
}