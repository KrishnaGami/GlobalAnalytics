using GlobalAnalytics.API.Controllers;
using GlobalAnalytics.API.Services.Auth;
using GlobalAnalytics.Core.Entities;
using GlobalAnalytics.Lib.DTOs;
using GlobalAnalytics.Lib.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace GlobalAnalytics.Test.Controllers
{
    [TestFixture]
    public class AuthControllerTests
    {
        private Mock<IUserRepository> _mockUserRepo;
        private JwtTokenService _jwtService;
        private AuthController _controller;

        [SetUp]
        public void Setup()
        {
            _mockUserRepo = new Mock<IUserRepository>();

            // Create mock IConfiguration
            var inMemorySettings = new Dictionary<string, string>
            {
                {"JwtSettings:Secret", "ThisIsASecretKeyOfAtLeast32Chars!!"},
                {"JwtSettings:Issuer", "TestIssuer"},
                {"JwtSettings:Audience", "TestAudience"},
                {"JwtSettings:AccessTokenExpiryMinutes", "60"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _jwtService = new JwtTokenService(configuration);
            _controller = new AuthController(_mockUserRepo.Object, _jwtService);
        }

        [Test]
        public async Task Login_ValidCredentials_ReturnsToken()
        {
            var loginDto = new LoginDto { Username = "admin", Password = "pass" };
            var user = new User { Id = 1, Username = "admin", Password = "pass", Role = "Admin" };

            _mockUserRepo.Setup(r => r.GetByUsernameAsync("admin")).ReturnsAsync(user);
            _mockUserRepo.Setup(r => r.SaveRefreshToken(user.Id, It.IsAny<string>())).Returns(Task.CompletedTask);

            var result = await _controller.Login(loginDto) as OkObjectResult;

            Assert.IsNotNull(result);
            var tokenResult = result.Value as TokenResponseDto;
            Assert.IsNotNull(tokenResult.AccessToken);
            Assert.IsNotNull(tokenResult.RefreshToken);
        }

        [Test]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            var loginDto = new LoginDto { Username = "wrong", Password = "wrong" };

            _mockUserRepo.Setup(r => r.GetByUsernameAsync("wrong")).ReturnsAsync((User)null);

            var result = await _controller.Login(loginDto);

            Assert.IsInstanceOf<UnauthorizedObjectResult>(result);
        }
    }
}
