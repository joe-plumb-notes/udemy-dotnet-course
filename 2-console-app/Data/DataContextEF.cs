

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyApp.Models;

namespace MyApp.Data
{
    public class DataContextEntityFramework : DbContext
    {
        private IConfiguration _config;
        public DataContextEntityFramework(IConfiguration config)
        {
            _config = config;
        }
        public DbSet<Computer> Computer { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnection"),
                    optionsBuilder => optionsBuilder.EnableRetryOnFailure());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("TutorialAppSchema");
            
            modelBuilder.Entity<Computer>()
                //.HasNoKey();
                .HasKey(c => c.ComputerId);
                // .ToTable("Computer", "TutorialAppSchema");
        }

    }
}
