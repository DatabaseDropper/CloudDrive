using CloudDrive.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudDrive.Models.ViewModels
{
    public class FileViewModel
    {
        public FileViewModel()
        {

        }

        public FileViewModel(File file)
        {
            Id = file.Id;
            Name = file.UserFriendlyName;
            Size = file.Size;
            IsPublic = file.IsAccessibleForEveryone;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public long Size { get; set; }

        public bool IsPublic { get; set; }
    }
}
