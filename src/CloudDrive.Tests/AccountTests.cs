using CloudDrive.Database;
using CloudDrive.Models.Auth;
using CloudDrive.Models.Input;
using CloudDrive.Models.ViewModels;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CloudDrive.Tests
{
    public class AccountTests
    {
        private readonly Context _context;
        private static int Port = 5000;
        private RestClient rest;
        public AccountTests()
        {
            var config = new ConfigurationBuilder()
                .AddCommandLine(new string[0])
                .Build();

            var str = $"filename={Guid.NewGuid()}.db";
            
            config["Database:TestString"] = str;
            var url = $"http://localhost:{Port++}";
            rest = new RestClient(url);
            var builder = WebHost.CreateDefaultBuilder()
                         .UseConfiguration(config)
                         .UseStartup<FakeStartup>()
                         .UseKestrel()
                         .UseUrls(url)
                         .Build();

            var options = new DbContextOptionsBuilder().UseSqlite(str);
            _context = new Context(options.Options);

            builder.Start();
        }

        [Fact]
        public async Task EnsureAppCanStart()
        {
            // Arrange
            var request = new RestRequest("/", Method.GET);

            // Act
            var response = await rest.ExecuteAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(response.Content.Length > 500);
        }

        [Fact]
        public async Task RegisterTest_ShouldBeOk()
        {
            // Arrange
            var request = new RestRequest("api/v1/Account/Register", Method.POST, DataFormat.Json);

            var registerData = new RegisterInput
            {
                Email = "test@localhost.test",
                Login = "abcdefg12353",
                Password = "asd123423534",
                UserName = "user_test"
            };

            request.AddJsonBody(registerData);

            // Act
            var response = await rest.ExecuteAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("token", response.Content);

            var users = _context.Users.ToList();

            Assert.Single(users);
        }

        [Fact]
        public async Task RegisterTest_ShouldFail()
        {
            // Arrange
            var request = new RestRequest("api/v1/Account/Register", Method.POST, DataFormat.Json);

            var registerData = new RegisterInput
            {
                Email = null,
                Login = null,
                Password = "",
                UserName = "user_test"
            };

            request.AddJsonBody(registerData);
            // Act
            var response = await rest.ExecuteAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var users = _context.Users.ToList();

            Assert.Empty(users);
        }

        [Fact]
        public async Task Register_And_TryAccess_Authorized_Endpoint_ShouldBeOk()
        {
            // Arrange
            var request = new RestRequest("api/v1/Account/Register", Method.POST, DataFormat.Json);

            var registerData = new RegisterInput
            {
                Email = "test@localhost.test",
                Login = "abcdefg12353",
                Password = "asd123423534",
                UserName = "user_test"
            };

            request.AddJsonBody(registerData);

            // Act
            var response = await rest.ExecuteAsync(request);
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("token", response.Content);

            var users = _context.Users.ToList();

            Assert.Single(users);
            // Step 2
            var obj = JsonConvert.DeserializeObject<AuthToken>(response.Content);

            var request2 = new RestRequest($"api/v1/Folder/{obj.DiskInfo.FolderId}", Method.GET, DataFormat.Json);
            request2.AddHeader("Authorization", $"Bearer {obj.Token}");
            response = await rest.ExecuteAsync(request2);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var obj2 = JsonConvert.DeserializeObject<FolderContent>(response.Content);

            Assert.Empty(obj2.Files);
            Assert.Empty(obj2.Folders);
        }
    }
}
