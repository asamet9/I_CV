using System.Security.Claims;

using ICV.Application.DTOs.CvSection;
using ICV.Application.Interfaces.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ICV.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CvSectionController : ControllerBase
    {
        private readonly ICvSectionService _cvSectionService;

        public CvSectionController(ICvSectionService cvSectionService)
        {
            _cvSectionService = cvSectionService;
        }

        [HttpDelete("{sectionId:int}")]
        public async Task<IActionResult> Delete(int sectionId)
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

            var result = await _cvSectionService
                .DeleteAsync(sectionId, userId);

            if (!result)
                return NotFound();

            return NoContent();
        }


        [HttpPut("{sectionId:int}")]
        public async Task<IActionResult> Update(
    int sectionId,
    UpdateCvSectionRequestDto request)
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

            var result = await _cvSectionService.UpdateAsync(
                sectionId,
                request,
                userId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }


        [HttpGet("detail/{sectionId:int}")]
        public async Task<IActionResult> GetById(int sectionId)
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

            var result = await _cvSectionService
                .GetByIdAsync(sectionId, userId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }


        [HttpGet("{cvId:int}")]
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
                var result = await _cvSectionService
                    .GetAllAsync(cvId, userId);

                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }



        [HttpPost("{cvId:int}")]
        public async Task<IActionResult> Create(
            int cvId,
            CreateCvSectionRequestDto request)
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
                var result = await _cvSectionService.CreateAsync(
                    cvId,
                    request,
                    userId);

                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }
    }
}