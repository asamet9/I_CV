using System.Security.Claims;

using ICV.Application.DTOs.Cv;
using ICV.Application.Interfaces.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ICV.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CvController : ControllerBase
    {
        private readonly ICvService _cvService;

        public CvController(ICvService cvService)
        {
            _cvService = cvService;
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userIdClaim = User.FindFirst(
                ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            if (!int.TryParse(userIdClaim.Value, out var userId))
                return Unauthorized();

            var result = await _cvService.DeleteAsync(id, userId);

            if (!result)
                return NotFound();

            return NoContent();
        }



        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
    int id,
    UpdateCvRequestDto request)
        {
            var userIdClaim = User.FindFirst(
                ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            if (!int.TryParse(userIdClaim.Value, out var userId))
                return Unauthorized();

            var result = await _cvService.UpdateAsync(
                id,
                request,
                userId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userIdClaim = User.FindFirst(
                ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            if (!int.TryParse(userIdClaim.Value, out var userId))
                return Unauthorized();

            var result = await _cvService.GetByIdAsync(id, userId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }


        [HttpGet("my-cvs")]
        public async Task<IActionResult> GetMyCvs()
        {
            var userIdClaim = User.FindFirst(
                ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            if (!int.TryParse(userIdClaim.Value, out var userId))
                return Unauthorized();

            var result = await _cvService.GetMyCvsAsync(userId);

            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateCvRequestDto request)
        {
            var userIdClaim = User.FindFirst(
                ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            if (!int.TryParse(userIdClaim.Value, out var userId))
                return Unauthorized();

            var result = await _cvService.CreateAsync(
                request,
                userId);

            return Ok(result);
        }
    }
}