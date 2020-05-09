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
using RestSharp.Extensions;
using System;
using System.IO;
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
        private const string TestFilesFolderName = "Test_Files";

        public AccountTests()
        {
            var config = new ConfigurationBuilder()
                .AddCommandLine(new string[0])
                .Build();

            var path = Path.Combine(".", TestFilesFolderName);
            Directory.CreateDirectory(path);
            var str = $"filename={Guid.NewGuid()}.db";
            
            config["Database:TestString"] = str;
            config["Storage:StorageFolderPath"] = path;
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

        [Fact]
        public async Task Download_Your_File_ShouldBeOk()
        {
            // Arrange

            var registerData = new RegisterInput
            {
                Email = "test@localhost.test",
                Login = "abcdefg12353",
                Password = "asd123423534",
                UserName = "user_test"
            };

            var fileContent = "abc";
            var path = Path.Combine(TestFilesFolderName, "text7.txt");
            CreatePhysicalFile(path, fileContent);

            // create account 
            var request = new RestRequest("api/v1/Account/Register", Method.POST, DataFormat.Json);
            request.AddJsonBody(registerData);
            var response = await rest.ExecuteAsync(request);
            var response_data = JsonConvert.DeserializeObject<AuthToken>(response.Content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // upload file
            var fileRequest = new RestRequest($"api/v1/File/{response_data.DiskInfo.FolderId}", Method.POST);
            fileRequest.RequestFormat = DataFormat.Json;
            fileRequest.AddHeader("Content-Type", "multipart/form-data");
            fileRequest.AddFile("file", path);
            fileRequest.AddHeader("Authorization", $"Bearer {response_data.Token}");

            var fileResponse = await rest.ExecuteAsync(fileRequest);
            Assert.Equal(HttpStatusCode.OK, fileResponse.StatusCode);
            var fileResponseData = JsonConvert.DeserializeObject<FileViewModel>(fileResponse.Content);
            // download file

            var downloadPath = Path.Combine(TestFilesFolderName, "text7_download.txt");

            var downloadRequest = new RestRequest($"api/v1/File/{fileResponseData.Id}");
            downloadRequest.AddHeader("Authorization", $"Bearer {response_data.Token}");
            var downloadResponse = rest.DownloadData(downloadRequest);

            File.WriteAllBytes(downloadPath, downloadResponse);

            foreach (var file in Directory.GetParent(downloadPath).GetFiles())
            {
                Console.WriteLine(file.FullName);
            }

            var downloadedContent = File.ReadAllText(downloadPath);
            Assert.Equal(fileContent, downloadedContent);
        }

        private void CreatePhysicalFile(string path, string content)
        {
            File.WriteAllText(path, content);
        }
    }
}