using GlobalAnalytics.API.Controllers;
using GlobalAnalytics.API.Services.Auth;
using GlobalAnalytics.Core.Entities;
using GlobalAnalytics.Lib.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace GlobalAnalytics.Test.Auth
{
    [TestFixture]
    public class JwtTokenServiceTests
    {
        private AuthController _controller;
        private Mock<IUserRepository> _mockUserRepo;
        private JwtTokenService _jwtService;

        [SetUp]
        public void Setup()
        {
            _mockUserRepo = new Mock<IUserRepository>();

            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["JwtSettings:Secret"]).Returns("superstrongsecrettokenkey!@#1234567890");
            mockConfig.Setup(c => c["JwtSettings:Issuer"]).Returns("GlobalAnalytics");
            mockConfig.Setup(c => c["JwtSettings:Audience"]).Returns("GlobalAnalyticsUsers");
            mockConfig.Setup(c => c["JwtSettings:AccessTokenExpiryMinutes"]).Returns("60");

            _jwtService = new JwtTokenService(mockConfig.Object);

            _controller = new AuthController(_mockUserRepo.Object, _jwtService);
        }

        [Test]
        public void GenerateAccessToken_ReturnsToken()
        {
            var token = _jwtService.GenerateAccessToken(new User { Id = 1, Username = "admin", Role = "Admin" });

            Assert.IsNotNull(token);
            Assert.IsTrue(token.Contains("."));
        }
    }

}
