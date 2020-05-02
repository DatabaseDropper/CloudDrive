using CloudDrive.Models;
using CloudDrive.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudDrive.Database.Confiuration
{
    public class FolderConfiguration : IEntityTypeConfiguration<Folder>
    {
        public void Configure(EntityTypeBuilder<Folder> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(128);

            builder.HasMany(x => x.Folders).WithOne(x => x.ParentFolder).HasForeignKey(x => x.ParentFolderId);
            builder.HasMany(x => x.Files);

            builder.HasIndex(x => x.Id);
            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.ParentFolderId);
        }
    }
}
