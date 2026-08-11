using ICV.Application.DTOs.Profession;
using ICV.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ICV.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProfessionController : ControllerBase
    {
        private readonly IProfessionService _professionService;

        public ProfessionController(
            IProfessionService professionService)
        {
            _professionService = professionService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateProfessionRequestDto request)
        {
            var result = await _professionService
                .CreateAsync(request);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _professionService
                .GetAllAsync();

            return Ok(result);
        }

        [HttpGet("{professionId:int}")]
        public async Task<IActionResult> GetById(int professionId)
        {
            var result = await _professionService
                .GetByIdAsync(professionId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("{professionId:int}")]
        public async Task<IActionResult> Update(
            int professionId,
            UpdateProfessionRequestDto request)
        {
            var result = await _professionService
                .UpdateAsync(professionId, request);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{professionId:int}")]
        public async Task<IActionResult> Delete(int professionId)
        {
            var result = await _professionService
                .DeleteAsync(professionId);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}