using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Training_Assignment.DTOs;
using Training_Assignment.Services.Interfaces;

namespace Training_Assignment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Login endpoint. Returns JWT token if successful.
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var token = await _authService.LoginAsync(dto);
            if (token == null)
                return Unauthorized("Invalid credentials");

            return Ok(new { Token = token });
        }
    }
}
