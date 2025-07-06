using GlobalAnalytics.API.Services.Auth;
using GlobalAnalytics.Lib.DTOs;
using GlobalAnalytics.Lib.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAnalytics.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly JwtTokenService _jwtService;

        public AuthController(IUserRepository userRepo, JwtTokenService jwtService)
        {
            _userRepo = userRepo;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userRepo.GetByUsernameAsync(dto.Username);
            if (user == null || user.Password != dto.Password)
                return Unauthorized("Invalid credentials");

            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            await _userRepo.SaveRefreshToken(user.Id, refreshToken);

            return Ok(new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refresh(RefreshRequestDto dto)
        {
            var user = await _userRepo.GetUserByRefreshToken(dto.RefreshToken);
            if (user == null)
                return Unauthorized("Invalid refresh token");

            await _userRepo.InvalidateRefreshToken(dto.RefreshToken);

            var newAccessToken = _jwtService.GenerateAccessToken(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            await _userRepo.SaveRefreshToken(user.Id, newRefreshToken);

            return Ok(new TokenResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
    }
}