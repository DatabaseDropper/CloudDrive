using CloudDrive.Database;
using CloudDrive.Interfaces;
using CloudDrive.Miscs;
using CloudDrive.Models;
using CloudDrive.Models.Entities;
using CloudDrive.Models.Input;
using CloudDrive.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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

        public FileService(Context context, IFileSystem fileSystem, IOptions<StorageSettings> settings)
        {
            _context = context;
            _fileSystem = fileSystem;
            _settings = settings.Value;
        }

        public async Task<Result<FileDTO>> DownloadFileAsync(Guid FileId, User user)
        {
            var folder = await _context
                               .Folders
                               .Include(x => x.Folders)
                               .Include(x => x.Files)
                               .Include(x => x.AuthorizedUsers)
                               .FirstOrDefaultAsync(x => x.Files.Any(f => f.Id == FileId));

            if (folder == null)
                return new Result<FileDTO>(false, null, "File not found", ErrorType.NotFound);

            var file = folder.Files.SingleOrDefault(x => x.Id == FileId);

            if (file == null)
                return new Result<FileDTO>(false, null, "File not found", ErrorType.NotFound);

            if (user == null && !folder.IsAccessibleForEveryone && !file.IsAccessibleForEveryone)
            {
                return new Result<FileDTO>(false, null, "Unauthorized", ErrorType.Unauthorized);
            }

            var isOnSharedList = folder.AuthorizedUsers.Any(x => x.UserId == user.Id && x.AccessType == AccessEnum.Read);

            if (!isOnSharedList && file.UploadedById != user.Id)
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

        public async Task<Result<CreateFolderResult>> CreateFolderAsync(Guid id, CreateFolderInput input, User user)
        {
            if (user == null)
                return new Result<CreateFolderResult>(false, null, "Unauthorized", ErrorType.Unauthorized);

            if (input is null || id == Guid.Empty)
                return new Result<CreateFolderResult>(false, null, "Data is not received", ErrorType.BadRequest);


            var folder = await _context
                                .Folders
                                .Include(x => x.Folders)
                                .FirstOrDefaultAsync(x => x.Id == id);

            var disk = await _context
                                .Disks
                                .Include(x => x.Owner)
                                .FirstOrDefaultAsync(x => x.OwnerId == user.Id && x.Id == folder.DiskHintId);

            if (folder == null)
            {
                return new Result<CreateFolderResult>(false, null, "Parent folder not found", ErrorType.BadRequest);
            }
                   
            if (disk == null)
            {
                return new Result<CreateFolderResult>(false, null, "Disk not found", ErrorType.BadRequest);
            }

            var newFolder = new Folder(folder, user.Id, disk.Id, input.Name);

            await _context.AddAsync(newFolder);

            await _context.SaveChangesAsync();

            var result = new CreateFolderResult { Id = newFolder.Id, Name = newFolder.Name };

            return new Result<CreateFolderResult>(true, result);
        }

        public async Task<Result<FileViewModel>> UploadFileAsync(Guid id, IFormFile file, User user)
        {
            if (user == null)
                return new Result<FileViewModel>(false, null, "Unauthorized", ErrorType.Unauthorized);

            if (file is null)
                return new Result<FileViewModel>(false, null, "File is not sent", ErrorType.BadRequest);

            var folder = await _context
                               .Folders
                               .Include(x => x.AuthorizedUsers)
                               .Include(x => x.Files)
                               .FirstOrDefaultAsync(x => x.Id == id);

            var canAccess = folder.AuthorizedUsers.Any(x => x.UserId == user.Id && x.AccessType == AccessEnum.Write);

            if (!canAccess && folder.OwnerId != user.Id)
            {
                return new Result<FileViewModel>(false, null, "Unauthorized", ErrorType.Unauthorized);
            }

            var folderDisk = await _context.Disks.FirstOrDefaultAsync(x => x.Id == folder.DiskHintId);

            if (folderDisk == null || folderDisk.FreeSpace < file.Length)
            {
                return new Result<FileViewModel>(false, null, "Avaliable disk size is smaller than yours file size.", ErrorType.BadRequest);
            }

            var newFileName = Guid.NewGuid().ToString("N");
            var path = Path.Combine(_settings.StorageFolderPath, newFileName);

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var result = await _fileSystem.TrySaveFile(path, memoryStream.ToArray());

                if (!result.Success)
                {
                    return new Result<FileViewModel>(false, null, result.Error, ErrorType.Internal);
                }
            }

            var newFile = new Models.Entities.File(file.FileName, newFileName, file.Length, user.Id);
            folderDisk.UsedSpace += file.Length;
            folder.Files.Add(newFile);
            await _context.AddAsync(newFile);

            await _context.SaveChangesAsync();

            return new Result<FileViewModel>(true, new FileViewModel (newFile.Id, newFile.UserFriendlyName, newFile.Size));
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
                return new Result<FolderContent>(false, null, "Unauthorized.", ErrorType.Unauthorized);

            if (user.Id == folder.OwnerId || folder.AuthorizedUsers.Any(x => x.UserId == user.DiskId))
            {
                return MapFolderToViewModel(folder);
            }

            return new Result<FolderContent>(false, null, "Unauthorized.", ErrorType.Unauthorized);
        }

        private static Result<FolderContent> MapFolderToViewModel(Folder folder)
        {
            var files = folder.Files.Select(x => new FileViewModel(x.Id, x.UserFriendlyName, x.Size)).ToList();
            var folders = folder.Folders.Select(x => new FolderViewModel(x.Id, x.Name, x.Folders.Count + x.Files.Count)).ToList();
            return new Result<FolderContent>(true, new FolderContent(files, folders));
        }
    }
}
