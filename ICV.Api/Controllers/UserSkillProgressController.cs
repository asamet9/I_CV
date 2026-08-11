
using System.Security.Claims;
using ICV.Application.DTOs.UserSkillProgress;
using ICV.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ICV.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserSkillProgressController : ControllerBase
    {
        private readonly IUserSkillProgressService _userSkillProgressService;

        public UserSkillProgressController(
            IUserSkillProgressService userSkillProgressService)
        {
            _userSkillProgressService = userSkillProgressService;
        }

        // Giriş yapan kullanıcının JWT içindeki ID'sini alır.
        private bool TryGetUserId(out int userId)
        {
            userId = 0;

            var userIdClaim = User.FindFirst(
                ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return false;

            return int.TryParse(
                userIdClaim.Value,
                out userId);
        }

        // Yeni skill ilerleme kaydı oluşturur.
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateUserSkillProgressRequestDto request)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized();

            try
            {
                var result = await _userSkillProgressService
                    .CreateAsync(request, userId);

                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        // Giriş yapan kullanıcının tüm skill ilerlemelerini getirir.
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized();

            var result = await _userSkillProgressService
                .GetAllAsync(userId);

            return Ok(result);
        }

        // Tek bir ilerleme kaydını getirir.
        [HttpGet("{progressId:int}")]
        public async Task<IActionResult> GetById(
            int progressId)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized();

            var result = await _userSkillProgressService
                .GetByIdAsync(progressId, userId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // Skill ilerleme kaydını günceller.
        [HttpPut("{progressId:int}")]
        public async Task<IActionResult> Update(
            int progressId,
            UpdateUserSkillProgressRequestDto request)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized();

            var result = await _userSkillProgressService
                .UpdateAsync(
                    progressId,
                    request,
                    userId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // Skill ilerleme kaydını siler.
        [HttpDelete("{progressId:int}")]
        public async Task<IActionResult> Delete(
            int progressId)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized();

            var result = await _userSkillProgressService
                .DeleteAsync(progressId, userId);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
