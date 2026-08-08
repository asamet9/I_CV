using ICV.Application.DTOs.User;
using ICV.Application.Interfaces.Services;

using Microsoft.AspNetCore.Mvc;

namespace ICV.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            var result = await _userService.RegisterAsync(request);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
    int id,
    UpdateUserRequestDto request)
        {
            var user = await _userService.UpdateAsync(id, request);

            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.DeleteAsync(id);

            if (!result)
                return NotFound("Kullanıcı bulunamadı.");

            return NoContent();
        }



    }
}