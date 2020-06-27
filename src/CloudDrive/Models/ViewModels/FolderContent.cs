using CloudDrive.Models.Entities;
using System;
using System.Collections.Generic;

namespace CloudDrive.Models.ViewModels
{
    public class FolderContent
    {
        public FolderContent()
        {

        }

        public FolderContent(Folder folder, List<FileViewModel> files, List<FolderContent> folders)
        {
            Id = folder.Id;
            Name = folder.Name;
            ParentId = folder.ParentFolderId;
            ParentName = folder.ParentFolder?.Name;
            Files = files;
            Folders = folders;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid? ParentId { get; set; }

        public string ParentName { get; set; }

        public List<FileViewModel> Files { get; set; } = new List<FileViewModel>();

        public List<FolderContent> Folders { get; set; } = new List<FolderContent>();
    }
}
