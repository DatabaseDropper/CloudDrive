using CloudDrive.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudDrive.Database.Confiuration
{
    public class UserAccessConfiguration : IEntityTypeConfiguration<UserAccess>
    {
        public void Configure(EntityTypeBuilder<UserAccess> builder)
        {
            builder.HasKey(x => new { x.UserId, x.FolderId });

            builder.HasOne(x => x.Folder).WithMany(x => x.AuthorizedUsers).HasForeignKey(x => x.FolderId);

            builder.HasOne(x => x.User).WithMany(x => x.SharedFolders).HasForeignKey(x => x.UserId);
        }
    }
}
