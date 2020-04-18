using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudDrive.Models.Auth
{
    public class LoginDiskInfo
    {
        public Guid DiskId { get; set; }

        public string DiskName { get; set; }

        public Guid FolderId { get; set; }
    }
}
