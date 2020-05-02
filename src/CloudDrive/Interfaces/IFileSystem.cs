using System.Threading.Tasks;

namespace CloudDrive.Interfaces
{
    public interface IFileSystem
    {
        Task<(bool Success, string Error)> TrySaveFile(string path, byte[] bytes);

        Task<(bool Success, string Error, byte[] bytes)> TryGetFile(string path);

        Task<(bool Success, string Error)> TryRemoveFile(string path);
    }
}
