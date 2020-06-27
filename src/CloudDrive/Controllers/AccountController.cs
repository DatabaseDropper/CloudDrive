using System;
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
    public class AccountController : BaseController
    {
        private readonly ITokenService _tokenService;
        private readonly AccountService _accountService;
        private readonly UserService _userService;

        public AccountController(ITokenService tokenService, AccountService accountService, UserService userService)
        {
            _tokenService = tokenService;
            _accountService = accountService;
            _userService = userService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterInput input)
        {
            var result = await _accountService.TryRegister(input);

            return result.Success ? Ok(result.Data) : (IActionResult)BadRequest(new { result.Errors });
        }     
        
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignInAsync([FromBody] LoginInput input)
        {
            var result = await _accountService.TrySignIn(input);

            return result.Success ? Ok(result.Data) : (IActionResult)BadRequest(new { result.Errors });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AccountInfo()
        {
            var user = await _userService.TryGetUserAsync(UserId());

            var result = await _accountService.ObtainAccountInfo(user);

            return result.Success ? Ok(result.Data) : (IActionResult)BadRequest("Something went wrong");
        }
    }
}
