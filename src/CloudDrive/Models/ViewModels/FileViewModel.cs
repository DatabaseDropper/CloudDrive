using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudDrive.Models.ViewModels
{
    public class FileViewModel
    {
        public FileViewModel()
        {

        }
        public FileViewModel(Guid id, string name, long sizeAsKB)
        {
            Id = id;
            Name = name;
            SizeAsKB = sizeAsKB;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public long SizeAsKB { get; set; }
    }
}
