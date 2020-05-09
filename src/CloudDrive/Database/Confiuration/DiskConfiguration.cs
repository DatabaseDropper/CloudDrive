using CloudDrive.Models;
using CloudDrive.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudDrive.Database.Confiuration
{
    public class DiskConfiguration : IEntityTypeConfiguration<Disk>
    {
        public void Configure(EntityTypeBuilder<Disk> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(128);
            builder.Property(x => x.TotalSpace).IsRequired();
            builder.Property(x => x.UsedSpace).IsRequired();

            builder.HasOne(x => x.Folder);

            builder.HasIndex(x => x.Id);
            builder.HasIndex(x => x.UsedSpace);
            builder.HasIndex(x => x.TotalSpace);
        }
    }
}
