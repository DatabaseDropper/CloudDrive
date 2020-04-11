using System;

namespace CloudDrive.Models
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

		public DateTime CreationDate { get; private set; } = DateTime.Now;

		public void ResizeDisk(long DesiredSize)
		{
			if (DesiredSize < 0)
				throw new Exception("Desired disk size must be greaeter or equal to 0");

			TotalSpaceAsKB = DesiredSize;
		}

		public static Disk CreateDisk(long SizeAsKB)
		{
			var disk = new Disk(SizeAsKB);
			var folder = new Folder();

			disk.Folder = folder;
			disk.FolderId = folder.Id;
			return disk;
		}
	}
}