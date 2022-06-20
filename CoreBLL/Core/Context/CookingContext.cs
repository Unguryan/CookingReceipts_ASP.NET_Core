using Core.Context.Dbo;
using Microsoft.EntityFrameworkCore;

namespace Core.Context
{
    public class CookingContext : DbContext
    {
        public CookingContext(DbContextOptions<CookingContext> options) : base(options)
        {

        }

        public DbSet<CategoryDbo> Categories { get; set; }

        public DbSet<ReceiptDbo> Receipts { get; set; }

        public DbSet<UserDbo> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryDbo>().ToTable("Categories");
            modelBuilder.Entity<ReceiptDbo>().ToTable("Receipts");
            modelBuilder.Entity<UserDbo>().ToTable("Users");


            modelBuilder.Entity<CategoryDbo>().HasKey(c => c.Id);
            modelBuilder.Entity<ReceiptDbo>().HasKey(c => c.Id);
            modelBuilder.Entity<UserDbo>().HasKey(c => c.Id);

            modelBuilder.Entity<UserDbo>().HasMany(c => c.Receipts).WithOne(c => c.Owner);
            modelBuilder.Entity<ReceiptDbo>().HasMany(c => c.Categories);
        }
    }
}
