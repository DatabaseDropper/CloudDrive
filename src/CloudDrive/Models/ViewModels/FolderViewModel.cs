using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudDrive.Models.ViewModels
{
    public class FolderViewModel
    {
        public FolderViewModel()
        {

        }

        public FolderViewModel(Guid id, string name, int itemsCount)
        {
            Id = id;
            Name = name;
            ItemsCount = itemsCount;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public int ItemsCount { get; set; }

        public List<FolderContent> Folders { get; set; } = new List<FolderContent>();
    }
}
