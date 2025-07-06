using GlobalAnalytics.Data.Services;
using GlobalAnalytics.Lib.DTOs;
using GlobalAnalytics.Lib.Interfaces;
using Moq;
using NUnit.Framework;

namespace GlobalAnalytics.Test.Services
{
    [TestFixture]
    public class ExportServiceTests
    {
        private Mock<IExporterFactory> _mockFactory;
        private Mock<IExporter> _mockExporter;
        private IExportService _service;

        [SetUp]
        public void Setup()
        {
            _mockFactory = new Mock<IExporterFactory>();
            _mockExporter = new Mock<IExporter>();
            _service = new ExportService(_mockFactory.Object);
        }

        [Test]
        public void Export_ReturnsExportedBytes()
        {
            var sampleData = new List<ClientDto> { new ClientDto { Name = "Client", Email = "client@example.com" } };
            _mockExporter.Setup(e => e.Export(sampleData)).Returns(new byte[] { 1, 2, 3 });

            _mockFactory.Setup(f => f.GetExporter("csv")).Returns(_mockExporter.Object);

            var result = _service.Export("csv", sampleData);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Length);
        }
    }

}
