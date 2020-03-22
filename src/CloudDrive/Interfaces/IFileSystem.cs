using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudDrive.Interfaces
{
    interface IFileSystem
    {
        public (bool Success, string Error) TrySaveFile(string path, byte[] bytes);

        public (bool Success, string Error, byte[] bytes) TryGetFile(string path);

        public (bool Success, string Error) TryRemoveFile(string path);
    }
}
