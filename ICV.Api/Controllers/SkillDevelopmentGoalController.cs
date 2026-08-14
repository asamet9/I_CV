using ICV.Application.DTOs.SkillDevelopmentGoal;
using ICV.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ICV.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SkillDevelopmentGoalController : ControllerBase
    {
        private readonly ISkillDevelopmentGoalService _skillDevelopmentGoalService;

        public SkillDevelopmentGoalController(
            ISkillDevelopmentGoalService skillDevelopmentGoalService)
        {
            _skillDevelopmentGoalService = skillDevelopmentGoalService;
        }

        // ---------------------------------------------------------
        // POST: api/SkillDevelopmentGoal
        // ---------------------------------------------------------

        /// <summary>
        /// Kullanıcı için yeni bir skill geliştirme hedefi oluşturur.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateSkillDevelopmentGoalRequestDto request)
        {
            var userId = GetUserId();

            var result = await _skillDevelopmentGoalService
                .CreateAsync(request, userId);

            return Ok(result);
        }

        // ---------------------------------------------------------
        // GET: api/SkillDevelopmentGoal
        // ---------------------------------------------------------

        /// <summary>
        /// Giriş yapan kullanıcının tüm skill geliştirme hedeflerini getirir.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserId();

            var result = await _skillDevelopmentGoalService
                .GetAllAsync(userId);

            return Ok(result);
        }

        // ---------------------------------------------------------
        // GET: api/SkillDevelopmentGoal/{id}
        // ---------------------------------------------------------

        /// <summary>
        /// Giriş yapan kullanıcının belirli bir skill geliştirme hedefini getirir.
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = GetUserId();

            var result = await _skillDevelopmentGoalService
                .GetByIdAsync(id, userId);

            if (result == null)
                return NotFound("Skill geliştirme hedefi bulunamadı.");

            return Ok(result);
        }

        // ---------------------------------------------------------
        // PUT: api/SkillDevelopmentGoal/{id}
        // ---------------------------------------------------------

        /// <summary>
        /// Kullanıcının kendi skill geliştirme hedefini günceller.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateSkillDevelopmentGoalRequestDto request)
        {
            var userId = GetUserId();

            var result = await _skillDevelopmentGoalService
                .UpdateAsync(id, request, userId);

            if (result == null)
                return NotFound("Skill geliştirme hedefi bulunamadı.");

            return Ok(result);
        }

        // ---------------------------------------------------------
        // DELETE: api/SkillDevelopmentGoal/{id}
        // ---------------------------------------------------------

        /// <summary>
        /// Kullanıcının kendi skill geliştirme hedefini siler.
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();

            var deleted = await _skillDevelopmentGoalService
                .DeleteAsync(id, userId);

            if (!deleted)
                return NotFound("Skill geliştirme hedefi bulunamadı.");

            return NoContent();
        }

        // ---------------------------------------------------------
        // USER ID
        // ---------------------------------------------------------

        /// <summary>
        /// JWT içerisindeki kullanıcı ID'sini alır.
        /// </summary>
        private int GetUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException(
                    "Kullanıcı kimliği bulunamadı.");
            }

            return userId;
        }
    }
}