using System;

namespace CloudDrive.Models
{
	public class Disk
	{
		public Guid Id { get; private set; } = Guid.NewGuid();

		public string Name { get; set; }

		public Guid FolderId { get; private set; }

		public Folder Folder { get; private set; }

		public long TotalSpaceAsKB { get; private set; }

		public long UsedSpaceAsKB { get; private set; }

		public long FreeSpaceAsKB => TotalSpaceAsKB - UsedSpaceAsKB;

		public DateTime CreationDate { get; private set; } = DateTime.Now;

		public void ResizeDisk(long DesiredSize)
		{
			if (DesiredSize < 0)
				throw new Exception("Desired disk size must be greaeter or equal to 0");

			TotalSpaceAsKB = DesiredSize;
		}
	}
}