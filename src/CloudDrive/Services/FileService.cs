using CloudDrive.Database;
using CloudDrive.Interfaces;
using CloudDrive.Miscs;
using CloudDrive.Models;
using CloudDrive.Models.Entities;
using CloudDrive.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CloudDrive.Services
{
    public class FileService
    {
        private readonly Context _context;
        private readonly IFileSystem _fileSystem;
        private readonly StorageSettings _settings;

        public FileService(Context context, IFileSystem fileSystem, StorageSettings settings)
        {
            _context = context;
            _fileSystem = fileSystem;
            _settings = settings;
        }

        public async Task<Result<FileDTO>> DownloadFileAsync(Guid FileId, User user)
        {
            var folder = await _context
                               .Folders
                               .Include(x => x.Folders)
                               .Include(x => x.Files)
                               .Include(x => x.AuthorizedUsers)
                               .FirstOrDefaultAsync(x => x.Files.Any(f => f.Id == FileId));

            var file = folder.Files.First(x => x.Id == FileId);

            if (user == null && !folder.IsAccessibleForEveryone && !file.IsAccessibleForEveryone)
            {
                return new Result<FileDTO>(false, null, "Unauthorized", ErrorType.Unauthorized);
            }

            var accessList = folder.AuthorizedUsers.Where(x => x.UserId == user.Id);

            if (accessList.Count() == 0 || !accessList.Any(x => x.AccessType == AccessEnum.Read))
            {
                return new Result<FileDTO>(false, null, "Unauthorized", ErrorType.Unauthorized);
            }

            var path = Path.Combine(_settings.StorageFolderPath, file.PhysicalFileName);

            var result = await _fileSystem.TryGetFile(path);

            if (!result.Success)
            {
                return new Result<FileDTO>(false, null, "Something went wrong, please try later.", ErrorType.Internal);
            }

            return new Result<FileDTO>(true, new FileDTO(result.Bytes, file.UserFriendlyName));
        }

        public async Task<Result<FileDTO>> UploadFileAsync(Guid id, IFormFile file, User user)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<FolderContent>> LoadFolderContentAsync(Guid FolderId, User user)
        {
            var folder = await _context
                               .Folders
                               .Include(x => x.Folders)
                               .Include(x => x.Files)
                               .Include(x => x.AuthorizedUsers)
                               .FirstOrDefaultAsync(x => x.Id == FolderId);

            if (folder == null)
                return new Result<FolderContent>(false, null, "Folder not found.", ErrorType.NotFound);

            if (folder.IsAccessibleForEveryone)
            {
                return MapFolderToViewModel(folder);
            }

            if (user == null)
                return new Result<FolderContent>(false, null, "Unauthorized.", ErrorType.BadRequest);

            if (user.Id == folder.OwnerId || folder.AuthorizedUsers.Any(x => x.UserId == user.DiskId))
            {
                return MapFolderToViewModel(folder);
            }

            return new Result<FolderContent>(false, null, "Unauthorized.", ErrorType.Unauthorized);
        }

        private static Result<FolderContent> MapFolderToViewModel(Folder folder)
        {
            var files = folder.Files.Select(x => new FileViewModel(x.Id, x.UserFriendlyName, x.SizeAsKB)).ToList();
            var folders = folder.Folders.Select(x => new FolderViewModel(x.Id, x.Name, x.Folders.Count + x.Files.Count)).ToList();
            return new Result<FolderContent>(true, new FolderContent(files, folders));
        }
    }
}
