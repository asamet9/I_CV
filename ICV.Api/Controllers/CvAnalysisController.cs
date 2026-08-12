using ICV.Application.DTOs.CvAnalysis;
using ICV.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ICV.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            // Şimdilik test amacıyla kullanıcı ID'sini sabitliyoruz.
            // Authentication eklediğimizde burası JWT'den gelecek.
            int userId = 4;

            var result = await _cvAnalysisService.AnalyzeAsync(
                cvId,
                request,
                userId);

            return Ok(result);
        }
    }
}