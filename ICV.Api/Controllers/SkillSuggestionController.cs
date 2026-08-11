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
    }
}
