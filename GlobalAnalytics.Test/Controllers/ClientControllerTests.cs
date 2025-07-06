using GlobalAnalytics.API.Controllers;
using GlobalAnalytics.Core.Models;
using GlobalAnalytics.Lib.DTOs;
using GlobalAnalytics.Lib.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
namespace GlobalAnalytics.Tests.Controllers
{
    [TestFixture]
    public class ClientControllerTests
    {
        private Mock<IClientService> _mockClientService;
        private Mock<IExportService> _mockExportService;
        private ClientController _controller;

        [SetUp]
        public void Setup()
        {
            _mockClientService = new Mock<IClientService>();
            _mockExportService = new Mock<IExportService>();

            _controller = new ClientController(_mockClientService.Object, _mockExportService.Object);
        }

        [Test]
        public async Task GetClients_ReturnsOkResult_WithPagedResult()
        {
            // Arrange
            var filter = new ClientFilterDto { PageSize = 5, PageNumber = 1 };

            _mockClientService.Setup(s => s.GetClientsAsync(filter)).ReturnsAsync(new PagedResult<ClientDto>
            {
                Data = new List<ClientDto> { new ClientDto {Name = "Test", Email = "a@test.com" } },
                TotalCount = 1
            });

            // Act
            var result = await _controller.GetClients(filter);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var pagedResult = okResult.Value as PagedResult<ClientDto>;
            Assert.NotNull(pagedResult);
            Assert.AreEqual(1, pagedResult.TotalCount);
        }
    }
}
