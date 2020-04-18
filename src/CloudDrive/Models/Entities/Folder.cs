using System;
using System.Collections.Generic;

namespace CloudDrive.Models
{
    public class Folder
	{
		public Guid Id { get; private set; } = Guid.NewGuid();

		public string Name { get; set; } = "Folder";

		public bool IsAccessibleForEveryone { get; set; } = false;

		public List<File> Files { get; private set; } = new List<File>();

		public List<Folder> Folders { get; private set; } = new List<Folder>();

		public Guid? ParentFolderId { get; private set; }

		public Folder ParentFolder { get; private set; }

		public List<UserAccess> AuthorizedUsers { get; private set; } = new List<UserAccess>();

		public Guid OwnerId { get; set; }
		
		public DateTime CreationDate { get; private set; } = DateTime.Now;
	}
}