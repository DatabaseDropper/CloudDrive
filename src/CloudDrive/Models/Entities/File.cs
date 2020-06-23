using System;

namespace CloudDrive.Models.Entities
{
    public class File
    {
        private File()
        {

        }

        public File(string userFriendlyName, string physicalFileName, long size, Guid uploadedById)
        {
            UserFriendlyName = userFriendlyName;
            PhysicalFileName = physicalFileName;
            Size = size;
            UploadedById = uploadedById;
        }

        public Guid Id { get; private set; } = Guid.NewGuid();

        public string UserFriendlyName { get; set; }

        public string PhysicalFileName { get; set; }

        public long Size { get; private set; }

        public bool IsAccessibleForEveryone { get; set; } = false;

        public Guid UploadedById { get; private set; }

        public User UploadedBy { get; private set; }

        public DateTime UploadedAt { get; private set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;
    }
}