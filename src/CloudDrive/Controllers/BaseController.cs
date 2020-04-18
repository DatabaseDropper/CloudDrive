using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace CloudDrive.Controllers
{
    public class BaseController : ControllerBase
    {
        public Guid? UserId()
        {
            var id = this.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;

            if (id == null)
                return null;

            return new Guid(id);
        }
    }
}
