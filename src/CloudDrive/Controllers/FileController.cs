using System;
using System.Threading.Tasks;
using CloudDrive.Miscs;
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
        public async Task<IActionResult> DownloadFile(Guid Id)
        {
            throw new Exception();
        }
    }
}
