using System;
using System.Threading.Tasks;
using CloudDrive.Miscs;
using CloudDrive.Models.Input;
using CloudDrive.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudDrive.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FolderController : BaseController
    {
        private readonly FileService _fileService;
        private readonly UserService _userService;

        public FolderController(FileService fileService, UserService userService)
        {
            _fileService = fileService;
            _userService = userService;
        }

        [HttpPost("{Id}")]
        public async Task<IActionResult> CreateFolder(Guid Id, CreateFolderInput input)
        {
            var user = await _userService.TryGetUserAsync(UserId());

            var result = await _fileService.CreateFolderAsync(Id, input, user);
            
            return result.Error switch
            {
                ErrorType.None => Ok(result.Data),
                ErrorType.NotFound => NotFound(result.Errors),
                ErrorType.Unauthorized => Unauthorized(result.Errors),
                _ => BadRequest()
            };
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteFolder(Guid Id)
        {
            var user = await _userService.TryGetUserAsync(UserId());

            var result = await _fileService.DeleteFolderAsync(user, Id);

            return result.Error switch
            {
                ErrorType.None => Ok(result.Data),
                ErrorType.NotFound => NotFound(result.Errors),
                ErrorType.Unauthorized => Unauthorized(result.Errors),
                _ => BadRequest()
            };
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> LoadUserFilesFromFolder(Guid Id)
        {
            var user = await _userService.TryGetUserAsync(UserId());

            var result = await _fileService.LoadFolderContentAsync(Id, user);

            return result.Error switch
            {
                ErrorType.None => Ok(result.Data),
                ErrorType.NotFound => NotFound(result.Errors),
                ErrorType.Unauthorized => Unauthorized(result.Errors),
                _ => BadRequest()
            };
        }

    }
}
