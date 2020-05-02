using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudDrive.Models.ViewModels
{
    public class FileDTO
    {
        public FileDTO()
        {

        }

        public FileDTO(byte[] bytes, string userFriendlyName)
        {
            Bytes = bytes;
            UserFriendlyName = userFriendlyName;
        }

        public byte[] Bytes { get; set; }
        public string UserFriendlyName { get; set; }
    }
}
