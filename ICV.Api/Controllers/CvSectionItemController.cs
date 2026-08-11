using System.Security.Claims;
using ICV.Application.DTOs.CvSectionItem;
using ICV.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ICV.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CvSectionItemController : ControllerBase
    {
        private readonly ICvSectionItemService _cvSectionItemService;

        public CvSectionItemController(
            ICvSectionItemService cvSectionItemService)
        {
            _cvSectionItemService = cvSectionItemService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateCvSectionItemRequestDto request)
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
                var result = await _cvSectionItemService
                    .CreateAsync(request, userId);

                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }

        }


        [HttpGet("{cvSectionId:int}")]
        public async Task<IActionResult> GetAll(int cvSectionId)
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
                var result = await _cvSectionItemService
                    .GetAllAsync(cvSectionId, userId);

                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpGet("detail/{itemId:int}")]
        public async Task<IActionResult> GetById(int itemId)
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

            var result = await _cvSectionItemService
                .GetByIdAsync(itemId, userId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }


        [HttpPut("{itemId:int}")]
        public async Task<IActionResult> Update(
    int itemId,
    UpdateCvSectionItemRequestDto request)
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

            var result = await _cvSectionItemService
                .UpdateAsync(itemId, request, userId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{itemId:int}")]
        public async Task<IActionResult> Delete(int itemId)
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

            var result = await _cvSectionItemService
                .DeleteAsync(itemId, userId);

            if (!result)
                return NotFound();

            return NoContent();
        }



    }
}