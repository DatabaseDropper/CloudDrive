using CloudDrive.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudDrive.Database.Confiuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Login).IsRequired().HasMaxLength(256);
            builder.Property(x => x.Username).IsRequired().HasMaxLength(32);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(128);
            builder.Property(x => x.PasswordHash).IsRequired();

            builder.HasOne(x => x.Disk).WithOne(x => x.Owner).HasForeignKey<Disk>(x => x.OwnerId);

            builder.HasIndex(x => x.Id);
        }
    }
}
