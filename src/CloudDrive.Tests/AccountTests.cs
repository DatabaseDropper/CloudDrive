using CloudDrive.Models.Input;
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
        public async Task EnsureAppCanStart()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
            string v = (await response.Content.ReadAsStringAsync());
            Assert.True(v.Length > 500);
        }     
        
        [Fact]
        public async Task RegisterTest_ShouldBeOk()
        {
            // Arrange
            var client = _factory.CreateClient();

            var registerData = new RegisterInput
            {
                Email = "test@localhost.test",
                Login = "abcdefg12353",
                Password = "asd123423534",
                UserName = "user_test"
            };

            // Act
            var response = await client.PostAsync("api/v1/Account/Register", registerData.ToPOSTableJSON());

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Contains("token", await response.Content.ReadAsStringAsync());
        }    
        
        [Fact]
        public async Task RegisterTest_ShouldFail()
        {
            // Arrange
            var client = _factory.CreateClient();

            var registerData = new RegisterInput
            {
                Email = null,
                Login = "abcdefg12353",
                Password = "1"
            };

            // Act
            var response = await client.PostAsync("api/v1/Account/Register", registerData.ToPOSTableJSON());
            var content = await response.Content.ReadAsStringAsync();
            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Contains("E-mail address is not correct", content);
            Assert.Contains("Password's length must be greater", content);
        }  
    }
}
