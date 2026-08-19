using System.Security.Claims;

using ICV.Application.DTOs.UserCvAnswer;
using ICV.Application.Interfaces.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ICV.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserCvAnswerController : ControllerBase
    {
        private readonly IUserCvAnswerService _userCvAnswerService;

        public UserCvAnswerController(
            IUserCvAnswerService userCvAnswerService)
        {
            _userCvAnswerService = userCvAnswerService;
        }

        // Kullanıcının CV'sine cevap ekler veya
        // mevcut cevabı günceller.
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateUserCvAnswerRequestDto request)
        {
            try
            {
                var userId = GetUserId();

                var result = await _userCvAnswerService
                    .CreateAsync(userId, request);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Kullanıcının belirli CV'sindeki
        // bütün cevapları getirir.
        [HttpGet("cv/{cvId:int}")]
        public async Task<IActionResult> GetByCvId(int cvId)
        {
            try
            {
                var userId = GetUserId();

                var result = await _userCvAnswerService
                    .GetByCvIdAsync(userId, cvId);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Tek bir cevabı getirir.
        [HttpGet("{answerId:int}")]
        public async Task<IActionResult> GetById(int answerId)
        {
            var userId = GetUserId();

            var result = await _userCvAnswerService
                .GetByIdAsync(userId, answerId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // Cevabı siler.
        [HttpDelete("{answerId:int}")]
        public async Task<IActionResult> Delete(int answerId)
        {
            var userId = GetUserId();

            var result = await _userCvAnswerService
                .DeleteAsync(userId, answerId);

            if (!result)
                return NotFound();

            return NoContent();
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedAccessException(
                    "Geçerli kullanıcı bulunamadı.");

            return userId;
        }
    }
}

