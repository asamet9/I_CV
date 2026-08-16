using System.Security.Claims;
using ICV.Application.DTOs.SkillSuggestion;
using ICV.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ICV.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SkillSuggestionController : ControllerBase
    {
        private readonly ISkillSuggestionService _skillSuggestionService;

        public SkillSuggestionController(
            ISkillSuggestionService skillSuggestionService)
        {
            _skillSuggestionService = skillSuggestionService;
        }

        // Yeni skill önerisi oluşturur.
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateSkillSuggestionRequestDto request)
        {
            var userIdClaim = User.FindFirst(
                ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            if (!int.TryParse(
                    userIdClaim.Value,
                    out var userId))
            {
                return Unauthorized();
            }

            try
            {
                var result = await _skillSuggestionService
                    .CreateAsync(request, userId);

                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        // Belirli bir CV'ye ait tüm skill önerilerini getirir.
        [HttpGet("cv/{cvId:int}")]
        public async Task<IActionResult> GetAll(int cvId)
        {
            var userIdClaim = User.FindFirst(
                ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            if (!int.TryParse(
                    userIdClaim.Value,
                    out var userId))
            {
                return Unauthorized();
            }

            try
            {
                var result = await _skillSuggestionService
                    .GetAllAsync(cvId, userId);

                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        // Tek bir skill önerisini getirir.
        [HttpGet("{suggestionId:int}")]
        public async Task<IActionResult> GetById(
            int suggestionId)
        {
            var userIdClaim = User.FindFirst(
                ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            if (!int.TryParse(
                    userIdClaim.Value,
                    out var userId))
            {
                return Unauthorized();
            }

            var result = await _skillSuggestionService
                .GetByIdAsync(suggestionId, userId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // Skill önerisini günceller.
        [HttpPut("{suggestionId:int}")]
        public async Task<IActionResult> Update(
            int suggestionId,
            UpdateSkillSuggestionRequestDto request)
        {
            var userIdClaim = User.FindFirst(
                ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            if (!int.TryParse(
                    userIdClaim.Value,
                    out var userId))
            {
                return Unauthorized();
            }

            var result = await _skillSuggestionService
                .UpdateAsync(
                    suggestionId,
                    request,
                    userId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // Skill önerisini siler.
        [HttpDelete("{suggestionId:int}")]
        public async Task<IActionResult> Delete(
            int suggestionId)
        {
            var userIdClaim = User.FindFirst(
                ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            if (!int.TryParse(
                    userIdClaim.Value,
                    out var userId))
            {
                return Unauthorized();
            }

            var result = await _skillSuggestionService
                .DeleteAsync(suggestionId, userId);

            if (!result)
                return NotFound();

            return NoContent();
        }

        // Gemini AI kullanarak CV için yeni skill önerileri oluşturur.
        [HttpPost("ai-generate")]
        public async Task<IActionResult> GenerateFromAi(
            [FromBody] GenerateAiSkillSuggestionRequestDto request)
        {
            // JWT içerisinden giriş yapan kullanıcının ID'sini alıyoruz.
            var userIdClaim = User.FindFirst(
                ClaimTypes.NameIdentifier);

            // Kullanıcı ID'si JWT içerisinde yoksa yetkisiz cevap döndürüyoruz.
            if (userIdClaim == null)
                return Unauthorized();

            // JWT içerisindeki kullanıcı ID'sini integer'a çevirmeyi deniyoruz.
            if (!int.TryParse(
                    userIdClaim.Value,
                    out var userId))
            {
                return Unauthorized();
            }

            try
            {
                // SkillSuggestionService üzerinden
                // Gemini AI analizini başlatıyoruz.
                var result = await _skillSuggestionService
                    .GenerateFromAiAsync(
                        request.CvId,
                        request.CvContent,
                        request.ProfessionName,
                        userId);

                // Oluşturulan skill önerilerini API response olarak döndürüyoruz.
                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                // Kullanıcı CV'nin sahibi değilse Forbidden döndürüyoruz.
                return Forbid();
            }
        }
    }
}
