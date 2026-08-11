
using System.Security.Claims;
using ICV.Application.DTOs.CourseRecommendation;
using ICV.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ICV.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CourseRecommendationController : ControllerBase
    {
        private readonly ICourseRecommendationService _courseRecommendationService;

        public CourseRecommendationController(
            ICourseRecommendationService courseRecommendationService)
        {
            _courseRecommendationService = courseRecommendationService;
        }

        // Yeni kurs önerisi oluşturur.
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateCourseRecommendationRequestDto request)
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
                var result = await _courseRecommendationService
                    .CreateAsync(request, userId);

                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        // Belirli bir SkillSuggestion'a ait kursları getirir.
        [HttpGet("skill-suggestion/{skillSuggestionId:int}")]
        public async Task<IActionResult> GetAll(
            int skillSuggestionId)
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
                var result = await _courseRecommendationService
                    .GetAllAsync(skillSuggestionId, userId);

                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        // Tek bir kurs önerisini getirir.
        [HttpGet("{courseRecommendationId:int}")]
        public async Task<IActionResult> GetById(
            int courseRecommendationId)
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

            var result = await _courseRecommendationService
                .GetByIdAsync(courseRecommendationId, userId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // Kurs önerisini günceller.
        [HttpPut("{courseRecommendationId:int}")]
        public async Task<IActionResult> Update(
            int courseRecommendationId,
            UpdateCourseRecommendationRequestDto request)
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

            var result = await _courseRecommendationService
                .UpdateAsync(
                    courseRecommendationId,
                    request,
                    userId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // Kurs önerisini siler.
        [HttpDelete("{courseRecommendationId:int}")]
        public async Task<IActionResult> Delete(
            int courseRecommendationId)
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

            var result = await _courseRecommendationService
                .DeleteAsync(courseRecommendationId, userId);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}

