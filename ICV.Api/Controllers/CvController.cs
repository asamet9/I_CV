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