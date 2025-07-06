using GlobalAnalytics.Core.Entities;
using GlobalAnalytics.Core.Filters;
using GlobalAnalytics.Data;
using GlobalAnalytics.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace GlobalAnalytics.Test.Repositories
{
    [TestFixture]
    public class ClientRepositoryTests
    {
        private AppDbContext _context;
        private ClientRepository _repo;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GlobalAnalytics")
                .EnableSensitiveDataLogging() // Helpful for debugging entity issues
                .Options;

            _context = new AppDbContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.Clients.AddRange(new List<Client>
            {
                new Client
                {
                    Id = 1,
                    Name = "ABC",
                    Country = "US",
                    Industry = "Tech",
                    Revenue = 1000,
                    City = "New York",
                    Email = "abc@example.com",
                    Phone = "1234567890",
                    PostalCode = "10001",
                    IsActive = true,
                    ClientSince = new System.DateTime(2020, 1, 1),
                    CreatedDate = System.DateTime.UtcNow,
                    UpdatedDate = System.DateTime.UtcNow
                },
                new Client
                {
                    Id = 2,
                    Name = "XYZ",
                    Country = "US",
                    Industry = "Finance",
                    Revenue = 2000,
                    City = "Los Angeles",
                    Email = "xyz@example.com",
                    Phone = "9876543210",
                    PostalCode = "90001",
                    IsActive = true,
                    ClientSince = new System.DateTime(2021, 3, 15),
                    CreatedDate = System.DateTime.UtcNow,
                    UpdatedDate = System.DateTime.UtcNow
                },
                new Client
                {
                    Id = 3,
                    Name = "PQR",
                    Country = "India",
                    Industry = "Tech",
                    Revenue = 1500,
                    City = "Mumbai",
                    Email = "pqr@example.com",
                    Phone = "9988776655",
                    PostalCode = "400001",
                    IsActive = true,
                    ClientSince = new System.DateTime(2022, 7, 10),
                    CreatedDate = System.DateTime.UtcNow,
                    UpdatedDate = System.DateTime.UtcNow
                }
            });

            _context.SaveChanges();
            _repo = new ClientRepository(_context);
        }

        [Test]
        public async Task GetClientsAsync_FilterByCountry_ReturnsCorrectClients()
        {
            var filter = new ClientFilter
            {
                Country = "US",
                PageNumber = 1,
                PageSize = 10
            };

            var result = await _repo.GetClientsAsync(filter);

            Assert.AreEqual(2, result.TotalCount);
            Assert.IsTrue(result.Data.All(c => c.Country == "US"));
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}
