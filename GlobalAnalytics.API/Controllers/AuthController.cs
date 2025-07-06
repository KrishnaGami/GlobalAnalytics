using GlobalAnalytics.API.Services.Auth;
using GlobalAnalytics.Lib.DTOs;
using GlobalAnalytics.Lib.Interfaces;
using log4net;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAnalytics.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly JwtTokenService _jwtService;
        private readonly ILog _logger = LogManager.GetLogger(typeof(AuthController));

        public AuthController(IUserRepository userRepo, JwtTokenService jwtService)
        {
            _userRepo = userRepo;
            _jwtService = jwtService;
        }

        /// <summary>
        /// Authenticate user and return JWT + Refresh Token.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            _logger.Info($"Login attempt for user: {dto.Username}");

            var user = await _userRepo.GetByUsernameAsync(dto.Username);
            if (user == null || user.Password != dto.Password)
            {
                _logger.Warn($"Login failed for user: {dto.Username}");
                return Unauthorized("Invalid credentials");
            }

            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            await _userRepo.SaveRefreshToken(user.Id, refreshToken);

            _logger.Info($"Login successful for user: {dto.Username}");

            return Ok(new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }

        /// <summary>
        /// Refresh token to get new access and refresh tokens.
        /// </summary>
        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refresh(RefreshRequestDto dto)
        {
            _logger.Info("Refresh token attempt received.");

            var user = await _userRepo.GetUserByRefreshToken(dto.RefreshToken);
            if (user == null)
            {
                _logger.Warn("Invalid refresh token received.");
                return Unauthorized("Invalid refresh token");
            }

            await _userRepo.InvalidateRefreshToken(dto.RefreshToken);

            var newAccessToken = _jwtService.GenerateAccessToken(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            await _userRepo.SaveRefreshToken(user.Id, newRefreshToken);

            _logger.Info($"Refresh token successful for user: {user.Username}");

            return Ok(new TokenResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
    }
}
