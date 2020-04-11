using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using Xunit;

namespace CloudDrive.Tests
{
    public class UnitTest1 : IClassFixture<CustomWebApplicationFactory<FakeStartup>>
    {
        private readonly CustomWebApplicationFactory<FakeStartup> _factory;

        public UnitTest1(CustomWebApplicationFactory<FakeStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Test1()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("https://localhost:5001/");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
    }
}
