using System;
using System.Net.Mime;
using System.Threading.Tasks;
using CloudDrive.Miscs;
using CloudDrive.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CloudDrive.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FileController : BaseController
    {
        private readonly FileService _fileService;
        private readonly UserService _userService;

        public FileController(FileService fileService, UserService userService)
        {
            _fileService = fileService;
            _userService = userService;
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> DownloadFile(Guid Id)
        {
            var user = await _userService.TryGetUserAsync(UserId());
            var result = await _fileService.DownloadFileAsync(Id, user);

            var mime = "";

            if (result.Error == ErrorType.None)
            {
                new FileExtensionContentTypeProvider().TryGetContentType(result.Data.UserFriendlyName, out mime);
                mime = mime ?? MediaTypeNames.Application.Octet;
            }

            return result.Error switch
            {
                ErrorType.Unauthorized => Unauthorized(result.Errors),
                ErrorType.Internal => StatusCode(StatusCodes.Status500InternalServerError ,result.Errors),
                ErrorType.None => File(result.Data.Bytes, mime),
                _ => BadRequest()
            };
        }      
        
        [HttpPost("{Id}")]
        public async Task<IActionResult> UploadFile(Guid Id, [FromForm] IFormFile file)
        {
            var user = await _userService.TryGetUserAsync(UserId());

            var result = await _fileService.UploadFileAsync(Id, file, user);

            return result.Error switch
            {
                ErrorType.Unauthorized => Unauthorized(result.Errors),
                ErrorType.Internal => StatusCode(StatusCodes.Status500InternalServerError ,result.Errors),
                ErrorType.None => Ok(result.Data),
                _ => BadRequest()
            };
        }  
        
        [HttpPost("Share/{Id}")]
        public async Task<IActionResult> ShareFile(Guid Id)
        {
            var user = await _userService.TryGetUserAsync(UserId());

            var result = await _fileService.ChangeFileShareAsync(Id, user);

            return result.Error switch
            {
                ErrorType.Unauthorized => Unauthorized(result.Errors),
                ErrorType.Internal => StatusCode(StatusCodes.Status500InternalServerError ,result.Errors),
                ErrorType.None => Ok(new { IsPrivate = result.Data }),
                _ => BadRequest()
            };
        }    
        
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteFile(Guid Id)
        {
            var user = await _userService.TryGetUserAsync(UserId());

            var result = await _fileService.DeleteFileAsync(user, Id);

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
