using CloudDrive.Database;
using CloudDrive.Miscs;
using CloudDrive.Models;
using CloudDrive.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CloudDrive.Services
{
    public class FileService
    {
        private readonly Context _context;

        public FileService(Context context)
        {
            _context = context;
        }

        public async Task<Result<FolderContent>> LoadFolderContentAsync(Guid FolderId, User user)
        {
            if (user == null)
                return new Result<FolderContent>(false, null, "Unauthorized.", ErrorType.BadRequest);

            var folder = await _context
                               .Folders
                               .Include(x => x.Folders)
                               .Include(x => x.Files)
                               .Include(x => x.AuthorizedUsers)
                               .FirstOrDefaultAsync(x => x.Id == FolderId);

            if (folder == null)
                return new Result<FolderContent>(false, null, "Folder not found.", ErrorType.NotFound);

            if (folder.IsAccessibleForEveryone || user.Id == folder.OwnerId || folder.AuthorizedUsers.Any(x => x.UserId == user.DiskId))
            {
                var files = folder.Files.Select(x => new FileViewModel(x.Id, x.UserFriendlyName, x.SizeAsKB)).ToList();
                var folders = folder.Folders.Select(x => new FolderViewModel(x.Id, x.Name, x.Folders.Count + x.Files.Count)).ToList();
                return new Result<FolderContent>(true, new FolderContent(files, folders));
            }

            return new Result<FolderContent>(false, null, "Unauthorized.", ErrorType.Unauthorized);
        }
    }
}
