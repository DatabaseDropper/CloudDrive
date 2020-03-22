using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CloudDrive.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AccountController : ControllerBase
    {
        public AccountController()
        {
        }

        [HttpGet]
        public IActionResult Test()
        {
            return Ok(DateTime.Now);
        }
    }
}
