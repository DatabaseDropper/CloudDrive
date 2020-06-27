using CloudDrive.Models;
using CloudDrive.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudDrive.Database.Confiuration
{
    public class FileConfiguration : IEntityTypeConfiguration<File>
    {
        public void Configure(EntityTypeBuilder<File> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserFriendlyName).IsRequired().HasMaxLength(128);
            builder.Property(x => x.PhysicalFileName).IsRequired().HasMaxLength(128);
            builder.Property(x => x.Size).IsRequired();

            builder.HasOne(x => x.UploadedBy);

            builder.HasIndex(x => x.Id);
            builder.HasIndex(x => x.UserFriendlyName);
            builder.HasIndex(x => x.UploadedById);

            builder.HasQueryFilter(x => x.IsDeleted == false);
        }
    }
}
