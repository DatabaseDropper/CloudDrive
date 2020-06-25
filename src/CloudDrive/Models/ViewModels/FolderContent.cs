using System;
using System.Collections.Generic;

namespace CloudDrive.Models.ViewModels
{
    public class FolderContent
    {
        public FolderContent()
        {

        }

        public FolderContent(Guid id, string name, List<FileViewModel> files, List<FolderContent> folders)
        {
            Id = id;
            Name = name;
            Files = files;
            Folders = folders;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<FileViewModel> Files { get; set; } = new List<FileViewModel>();

        public List<FolderContent> Folders { get; set; } = new List<FolderContent>();
    }
}
