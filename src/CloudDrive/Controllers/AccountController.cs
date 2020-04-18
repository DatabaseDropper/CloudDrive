using System;
using CloudDrive.Interfaces;
using CloudDrive.Models.Input;
using CloudDrive.Services;
using Microsoft.AspNetCore.Mvc;

namespace CloudDrive.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly AccountService _accountService;

        public AccountController(ITokenService tokenService, AccountService accountService)
        {
            _tokenService = tokenService;
            _accountService = accountService;
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterInput input)
        {
            _accountService.TryRegister(input);
            return Ok(DateTime.Now);
        }
    }
}
