using Microsoft.EntityFrameworkCore;
using Task_ValidationAndLogging.Data.Models;

namespace Task_ValidationAndLogging.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasIndex(x => x.Name).IsUnique();
        }
        public DbSet<Product> Products { get; set; }
        
    }
}
