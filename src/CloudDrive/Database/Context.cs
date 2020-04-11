using CloudDrive.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CloudDrive.Database
{
    public class Context : DbContext
    {
        public Context(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Disk> Disks { get; set; }

        public DbSet<File> Files { get; set; }

        public DbSet<Folder> Folders { get; set; }
    }
}
