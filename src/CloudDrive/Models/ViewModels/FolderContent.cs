using System.Collections.Generic;

namespace CloudDrive.Models.ViewModels
{
    public class FolderContent
    {
        public FolderContent()
        {

        }

        public FolderContent(List<FileViewModel> files, List<FolderViewModel> folders)
        {
            Files = files;
            Folders = folders;
        }

        public List<FileViewModel> Files { get; set; } = new List<FileViewModel>();

        public List<FolderViewModel> Folders { get; set; } = new List<FolderViewModel>();
    }
}
