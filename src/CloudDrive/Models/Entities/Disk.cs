using System;

namespace CloudDrive.Models.Entities
{
	public class Disk
	{
		private Disk()
		{

		}

		public Disk(long totalSpaceAsKB)
		{
			TotalSpaceAsKB = totalSpaceAsKB;
		}

		public Guid Id { get; private set; } = Guid.NewGuid();

		public string Name { get; set; } = "Disk";

		public Guid FolderId { get; private set; }

		public Folder Folder { get; private set; }

		public long TotalSpaceAsKB { get; private set; } = 0;

		public long UsedSpaceAsKB { get; private set; } = 0;

		public long FreeSpaceAsKB => TotalSpaceAsKB - UsedSpaceAsKB;

		public Guid OwnerId { get; private set; }

		public User Owner { get; private set; }

		public DateTime CreationDate { get; private set; } = DateTime.Now;

		public void ResizeDisk(long DesiredSize)
		{
			if (DesiredSize < 0)
				throw new Exception("Desired disk size must be greaeter or equal to 0");

			TotalSpaceAsKB = DesiredSize;
		}

		public static Disk CreateDisk(long SizeAsKB, User user)
		{
			var disk = new Disk(SizeAsKB);
			var folder = new Folder();
			disk.Owner = user;
			disk.OwnerId = user.Id;
			disk.Folder = folder;
			disk.FolderId = folder.Id;
			disk.Folder.OwnerId = user.Id;
			return disk;
		}
	}
}