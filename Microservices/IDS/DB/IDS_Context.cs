using IDS.DB.Models;
using Microsoft.EntityFrameworkCore;

namespace IDS.DB
{
    public class IDS_Context : DbContext
    {
        public IDS_Context(DbContextOptions<IDS_Context> options) : base(options)
        {

        }

        public DbSet<RoleIDS> Roles { get; set; }

        public DbSet<UserIDS> Users { get; set; }

        public DbSet<UserTokenIDS> Tokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleIDS>().ToTable("Roles");
            modelBuilder.Entity<UserIDS>().ToTable("Users");
            modelBuilder.Entity<UserTokenIDS>().ToTable("Tokens");

            modelBuilder.Entity<RoleIDS>().HasKey(c => c.Id);
            modelBuilder.Entity<UserIDS>().HasKey(c => c.Id);
            modelBuilder.Entity<UserTokenIDS>().HasKey(c => c.Id);

            //modelBuilder.Entity<RoleIDS>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            //modelBuilder.Entity<UserIDS>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            //modelBuilder.Entity<UserTokenIDS>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
        }
    }
}
