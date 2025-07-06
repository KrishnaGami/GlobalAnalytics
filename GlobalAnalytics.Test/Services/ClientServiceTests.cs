using AutoMapper;
using GlobalAnalytics.Core.Entities;
using GlobalAnalytics.Core.Filters;
using GlobalAnalytics.Core.Models;
using GlobalAnalytics.Data.Services;
using GlobalAnalytics.Lib.DTOs;
using GlobalAnalytics.Lib.Interfaces;
using Moq;
using NUnit.Framework;

namespace GlobalAnalytics.Test.Services
{
    [TestFixture]
    public class ClientServiceTests
    {
        private Mock<IClientRepository> _mockRepo;
        private Mock<IMapper> _mockMapper;
        private IClientService _clientService;

        [SetUp]
        public void SetUp()
        {
            _mockRepo = new Mock<IClientRepository>();
            _mockMapper = new Mock<IMapper>();
            _clientService = new ClientService(_mockRepo.Object, _mockMapper.Object);
        }

        [Test]
        public async Task GetClientsAsync_ReturnsMappedClients()
        {
            // Arrange
            var filterDto = new ClientFilterDto { Country = "USA" };
            var entityList = new List<Client> { new Client { Name = "Client", Email = "client@example.com" } };

            _mockRepo.Setup(r => r.GetClientsAsync(It.IsAny<ClientFilter>()))
                .ReturnsAsync(new PagedResult<Client> { Data = entityList, TotalCount = 1 });

            _mockMapper.Setup(m => m.Map<List<ClientDto>>(It.IsAny<List<Client>>()))
                .Returns(new List<ClientDto> { new ClientDto { Name = "Client", Email = "client@example.com" } });

            // Act
            var result = await _clientService.GetClientsAsync(filterDto);

            // Assert
            Assert.AreEqual(1, result.TotalCount);
            Assert.AreEqual("Client", result.Data[0].Name);
            Assert.AreEqual("client@example.com", result.Data[0].Email);
        }
    }

}
