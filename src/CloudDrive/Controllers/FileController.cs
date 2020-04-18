using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CloudDrive.Interfaces;
using CloudDrive.Models.Input;
using CloudDrive.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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


        [Authorize]
        [HttpGet("{Id}")]
        public async Task<IActionResult> RegisterAsync(Guid Id)
        {
            var user = await _userService.TryGetUserAsync(UserId().Value);

            var result = await _fileService.LoadFolderContentAsync(Id, user);

            return result.Success ? Ok(result.Data) : (IActionResult)BadRequest(result.Errors);
        }
    }
}
