using GlobalAnalytics.Core.Entities;
using log4net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GlobalAnalytics.API.Services.Auth
{
    /// <summary>
    /// Handles generation of JWT Access and Refresh tokens.
    /// </summary>
    public class JwtTokenService
    {
        private readonly IConfiguration _config;
        private readonly ILog _logger = LogManager.GetLogger(typeof(JwtTokenService));

        public JwtTokenService(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Generates a signed JWT Access Token for the given user.
        /// </summary>
        public string GenerateAccessToken(User user)
        {
            try
            {
                _logger.Info($"Generating access token for user: {user.Username}");

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var secretKey = _config["JwtSettings:Secret"];
                var keyBytes = Encoding.UTF8.GetBytes(secretKey);

                if (keyBytes.Length < 32)
                {
                    _logger.Error("JWT secret key is too short. It must be at least 256 bits (32 characters).");
                    throw new Exception("JWT secret key too short. Must be at least 32 characters.");
                }

                var key = new SymmetricSecurityKey(keyBytes);
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _config["JwtSettings:Issuer"],
                    audience: _config["JwtSettings:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(
                        Convert.ToDouble(_config["JwtSettings:AccessTokenExpiryMinutes"])),
                    signingCredentials: creds);

                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                _logger.Info($"Access token generated for user: {user.Username}");
                return jwt;
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to generate access token.", ex);
                throw;
            }
        }

        /// <summary>
        /// Generates a secure 64-byte refresh token.
        /// </summary>
        public string GenerateRefreshToken()
        {
            try
            {
                var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
                _logger.Info("Refresh token generated.");
                return token;
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to generate refresh token.", ex);
                throw;
            }
        }
    }
}
