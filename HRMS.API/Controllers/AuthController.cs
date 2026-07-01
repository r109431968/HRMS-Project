using Azure.Core;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces;
using HRMS.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(
                request.FullName,
                request.Email,
                request.Password);

            return Ok(new { message = result });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request.Email, request.Password);

            if (result == null)
                return Unauthorized(new { message = "Invalid email or password." });

            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var (newAccessToken, newRefreshToken) = await _authService.RefreshTokenAsync(request.RefreshToken);

                return Ok(new AuthResponseDto
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Invalid or expired refresh token." });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request)
        {
            var result = await _authService.RevokeTokenAsync(request.RefreshToken);

            if (!result)
                return BadRequest(new { message = "Invalid token." });

            return Ok(new { message = "Logged out successfully." });
        }
    }
}
