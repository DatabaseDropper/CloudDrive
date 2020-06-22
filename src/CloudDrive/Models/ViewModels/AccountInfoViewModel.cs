using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudDrive.Models.ViewModels
{
    public class AccountInfoViewModel
    {
        public string UserName { get; set; }

        public long TotalSpace { get; set; }

        public long UsedSpace { get; set; }

        public long FreeSpace { get; set; }

        public Guid MainFolderId { get; set; }

        public Guid DiskId { get; set; }

        public string DiskName { get; set; }
    }
}
