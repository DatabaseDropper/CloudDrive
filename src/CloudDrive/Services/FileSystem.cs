using CloudDrive.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CloudDrive.Services
{
    public class FileSystem : IFileSystem
    {
        public async Task<(bool Success, string Error, byte[] Bytes)> TryGetFile(string path)
        {
            if (!File.Exists(path))
            {
                return (false, "File not found", new byte[0]);
            }

            // race condition

            var bytes = await File.ReadAllBytesAsync(path);
            return (true, "", bytes);
        }

        public Task<(bool Success, string Error)> TryRemoveFile(string path)
        {
            if (!File.Exists(path))
            {
                // we do not need to check that, but...
                // it would indicate that something may be really bad :)
                // todo: log
                // compiler will remove that empty if anyway
            }

            File.Delete(path);
            return Task.FromResult((true, ""));
        }

        public async Task<(bool Success, string Error)> TrySaveFile(string path, byte[] bytes)
        {
            Console.WriteLine($"Saving file: {path}");
            await File.WriteAllBytesAsync(path, bytes);
            return (true, "");
        }
    }
}
