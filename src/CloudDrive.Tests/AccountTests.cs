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
            var str = $"filename={Guid.NewGuid()}.db;foreign keys=true;";
            
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
        public async Task Register_And_CreateNewFolderSuccessfully()
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

            var request2 = new RestRequest($"api/v1/Folder/{obj.DiskInfo.FolderId}", Method.POST, DataFormat.Json);
            request2.AddJsonBody(new CreateFolderInput { Name = "test" });
            request2.AddHeader("Authorization", $"Bearer {obj.Token}");
            response = await rest.ExecuteAsync(request2);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);       
            
            var request3 = new RestRequest($"api/v1/Folder/{obj.DiskInfo.FolderId}", Method.GET, DataFormat.Json);
            request3.AddHeader("Authorization", $"Bearer {obj.Token}");
            response = await rest.ExecuteAsync(request3);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var obj2 = JsonConvert.DeserializeObject<FolderContent>(response.Content);

            Assert.Empty(obj2.Files);
            Assert.NotEmpty(obj2.Folders);
            Assert.Contains(obj2.Folders, x => x.Name == "test");
            Assert.DoesNotContain(obj2.Folders, x => x.Name == "Folder");
        }    
        
        [Fact]
        public async Task Ensure_YouCannotDelete_Main_Folder()
        {
            // Step 1 - Register
            var request = new RestRequest("api/v1/Account/Register", Method.POST, DataFormat.Json);

            var registerData = new RegisterInput
            {
                Email = "test@localhost.test",
                Login = "abcdefg12353",
                Password = "asd123423534",
                UserName = "user_test"
            };

            request.AddJsonBody(registerData);

            var response = await rest.ExecuteAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("token", response.Content);

            var users = _context.Users.ToList();

            Assert.Single(users);

            // Step 2 - Try Delete Main Folder
            var obj = JsonConvert.DeserializeObject<AuthToken>(response.Content);

            var request2 = new RestRequest($"api/v1/Folder/{obj.DiskInfo.FolderId}", Method.DELETE, DataFormat.Json);
            request2.AddHeader("Authorization", $"Bearer {obj.Token}");
            response = await rest.ExecuteAsync(request2);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);       
        }     
        
        [Fact]
        public async Task Register_And_CreateTwoNestedFolders_SendFile_DeleteParentFolder()
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
            // Step 2 - Create Folder
            var obj = JsonConvert.DeserializeObject<AuthToken>(response.Content);

            var request2 = new RestRequest($"api/v1/Folder/{obj.DiskInfo.FolderId}", Method.POST, DataFormat.Json);
            request2.AddJsonBody(new CreateFolderInput { Name = "test" });
            request2.AddHeader("Authorization", $"Bearer {obj.Token}");
            response = await rest.ExecuteAsync(request2);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var step2Repsponse = JsonConvert.DeserializeObject<CreateFolderResult>(response.Content);

            // Step 3 - Create folder inside

            var request3 = new RestRequest($"api/v1/Folder/{step2Repsponse.Id}", Method.POST, DataFormat.Json);
            request3.AddJsonBody(new CreateFolderInput { Name = "test" });
            request3.AddHeader("Authorization", $"Bearer {obj.Token}");
            response = await rest.ExecuteAsync(request3);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var step3Repsponse = JsonConvert.DeserializeObject<CreateFolderResult>(response.Content);


            // Step 4 - Upload file to deeper folder

            var fileContent = "abc";
            var path = Path.Combine(TestFilesFolderName, "text7.txt");
            CreatePhysicalFile(path, fileContent);

            var fileRequest = new RestRequest($"api/v1/File/{step3Repsponse.Id}", Method.POST);
            fileRequest.RequestFormat = DataFormat.Json;
            fileRequest.AddHeader("Content-Type", "multipart/form-data");
            fileRequest.AddFile("file", path);
            fileRequest.AddHeader("Authorization", $"Bearer {obj.Token}");

            var fileResponse = await rest.ExecuteAsync(fileRequest);
            Assert.Equal(HttpStatusCode.OK, fileResponse.StatusCode);
         
            // Step 5 - Delete parent folder

            var deleteRequest = new RestRequest($"api/v1/Folder/{step2Repsponse.Id}", Method.DELETE, DataFormat.Json);
            deleteRequest.AddHeader("Authorization", $"Bearer {obj.Token}");
            response = await rest.ExecuteAsync(deleteRequest);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Step 6 - Ensure is empty

            var ensure_exists_request = new RestRequest($"api/v1/Folder/{obj.DiskInfo.FolderId}", Method.GET, DataFormat.Json);
            ensure_exists_request.AddHeader("Authorization", $"Bearer {obj.Token}");
            var ensure_exists_response = await rest.ExecuteAsync(ensure_exists_request);

            Assert.Equal(HttpStatusCode.OK, ensure_exists_response.StatusCode);

            var finalResult = JsonConvert.DeserializeObject<FolderContent>(ensure_exists_response.Content);

            Assert.Empty(finalResult.Files);
            Assert.Empty(finalResult.Folders);
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

            var downloadedContent = File.ReadAllText(downloadPath);
            Assert.Equal(fileContent, downloadedContent);
        }

        [Fact]
        public async Task Download_NonPrivate_File()
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

            // change status request
            var statusReq = new RestRequest($"api/v1/File/Share/{fileResponseData.Id}", Method.POST);
            statusReq.RequestFormat = DataFormat.Json;
            statusReq.AddHeader("Authorization", $"Bearer {response_data.Token}");
            var statusResponse = await rest.ExecuteAsync(statusReq);
            Assert.Equal(HttpStatusCode.OK, statusResponse.StatusCode);
            Assert.Contains("true", statusResponse.Content);

            // download file

            var downloadPath = Path.Combine(TestFilesFolderName, "text8_download.txt");
            var downloadRequest = new RestRequest($"api/v1/File/{fileResponseData.Id}");
            var downloadResponse = rest.DownloadData(downloadRequest);

            File.WriteAllBytes(downloadPath, downloadResponse);

            var downloadedContent = File.ReadAllText(downloadPath);
            Assert.Equal(fileContent, downloadedContent);
        }

        [Fact]
        public async Task Delete_File_Successfully()
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

            // assert file exists

            var ensure_exists_request = new RestRequest($"api/v1/Folder/{response_data.DiskInfo.FolderId}", Method.GET, DataFormat.Json);
            ensure_exists_request.AddHeader("Authorization", $"Bearer {response_data.Token}");
            var ensure_exists_response = await rest.ExecuteAsync(ensure_exists_request);

            Assert.Equal(HttpStatusCode.OK, ensure_exists_response.StatusCode);

            var obj2 = JsonConvert.DeserializeObject<FolderContent>(ensure_exists_response.Content);

            Assert.NotEmpty(obj2.Files);

            // get disk size

            var disk_info_request1 = new RestRequest($"api/v1/Account", Method.GET);
            disk_info_request1.AddHeader("Authorization", $"Bearer {response_data.Token}");
            var disk_info_response1 = await rest.ExecuteAsync(disk_info_request1);

            Assert.Equal(HttpStatusCode.OK, disk_info_response1.StatusCode);

            var diskResult1 = JsonConvert.DeserializeObject<AccountInfoViewModel>(disk_info_response1.Content);

            // delete file

            var delete_request = new RestRequest($"api/v1/File/{fileResponseData.Id}", Method.DELETE);
            delete_request.AddHeader("Authorization", $"Bearer {response_data.Token}");
            var deleteResponse = await rest.ExecuteAsync(delete_request);

            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);

            // assert file is removed

            var ensure_deleted_request = new RestRequest($"api/v1/Folder/{response_data.DiskInfo.FolderId}", Method.GET, DataFormat.Json);
            ensure_deleted_request.AddHeader("Authorization", $"Bearer {response_data.Token}");
            var ensure_deleted_response = await rest.ExecuteAsync(ensure_deleted_request);

            Assert.Equal(HttpStatusCode.OK, ensure_deleted_response.StatusCode);

            obj2 = JsonConvert.DeserializeObject<FolderContent>(ensure_deleted_response.Content);

            Assert.Empty(obj2.Files);

            // get disk size

            var disk_info_request2 = new RestRequest($"api/v1/Account", Method.GET);
            disk_info_request2.AddHeader("Authorization", $"Bearer {response_data.Token}");
            var disk_info_response2 = await rest.ExecuteAsync(disk_info_request2);

            Assert.Equal(HttpStatusCode.OK, disk_info_response1.StatusCode);

            var diskResult2 = JsonConvert.DeserializeObject<AccountInfoViewModel>(disk_info_response2.Content);

            Assert.True(diskResult2.FreeSpace > diskResult1.FreeSpace);
            Assert.True(diskResult2.UsedSpace < diskResult1.UsedSpace);
        }

        [Fact]
        public async Task RegisterTest_AndGetAccountInfo_ShouldBeOk()
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

            var response_data = JsonConvert.DeserializeObject<AuthToken>(response.Content);

            var users = _context.Users.ToList();

            Assert.Single(users);
            
            // ______________________________________

            var request2 = new RestRequest("api/v1/Account/", Method.GET);
            request2.AddHeader("Authorization", $"Bearer {response_data.Token}");
            var response2 = await rest.ExecuteAsync(request2);
            var data = JsonConvert.DeserializeObject<AccountInfoViewModel>(response2.Content);

            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);

            Assert.Equal("Disk", data.DiskName);
            Assert.Equal("user_test", data.UserName);
            Assert.True(data.MainFolderId != Guid.Empty);
            Assert.True(data.DiskId != Guid.Empty);
            Assert.True(data.UsedSpace == 0);
            Assert.True(data.FreeSpace > 0);
            Assert.True(data.TotalSpace > 0);
        }

        private void CreatePhysicalFile(string path, string content)
        {
            File.WriteAllText(path, content);
        }
    }
}