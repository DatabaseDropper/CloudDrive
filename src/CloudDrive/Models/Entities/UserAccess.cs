using System;

namespace CloudDrive.Models.Entities
{
    public class UserAccess
    {
        public Guid UserId { get; set; }

        public User User { get; set; }

        public Guid FolderId { get; set; }

        public Folder Folder { get; set; }

        public AccessEnum AccessType { get; internal set; }
    }
}