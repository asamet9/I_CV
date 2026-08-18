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


        // =========================================================
        // AI İLE GELİŞİM HEDEFİNE KURS ÖNERİLERİ ÜRET
        // =========================================================

        [HttpPost("generate/{skillDevelopmentGoalId:int}")]
        public async Task<IActionResult> GenerateForGoal(
            int skillDevelopmentGoalId)
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
                var result =
                    await _courseRecommendationService
                        .GenerateForGoalAsync(
                            skillDevelopmentGoalId,
                            userId);

                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }


        // =========================================================
        // MANUEL KURS ÖNERİSİ OLUŞTUR
        // =========================================================

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
                var result =
                    await _courseRecommendationService
                        .CreateAsync(
                            request,
                            userId);

                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }


        // =========================================================
        // GELİŞİM HEDEFİNİN KURSLARINI GETİR
        // =========================================================

        [HttpGet(
            "skill-development-goal/{skillDevelopmentGoalId:int}")]
        public async Task<IActionResult> GetAll(
            int skillDevelopmentGoalId)
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
                var result =
                    await _courseRecommendationService
                        .GetAllAsync(
                            skillDevelopmentGoalId,
                            userId);

                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }


        // =========================================================
        // TEK KURS ÖNERİSİ
        // =========================================================

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

            var result =
                await _courseRecommendationService
                    .GetByIdAsync(
                        courseRecommendationId,
                        userId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }


        // =========================================================
        // KURS ÖNERİSİNİ GÜNCELLE
        // =========================================================

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

            var result =
                await _courseRecommendationService
                    .UpdateAsync(
                        courseRecommendationId,
                        request,
                        userId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }


        // =========================================================
        // KURS ÖNERİSİNİ SİL
        // =========================================================

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

            var result =
                await _courseRecommendationService
                    .DeleteAsync(
                        courseRecommendationId,
                        userId);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}