using DotnetAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Data
{
    public class DataContextEF : DbContext
    {
        private readonly IConfiguration _config;

        public DataContextEF(IConfiguration config)
        {
            _config = config; 
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserJobInfo> UserJobInfo { get; set; }
        public virtual DbSet<UserSalary> UserSalary { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(_config.GetConnectionString("DefaultConnection"),
                        optionsBuilder => optionsBuilder.EnableRetryOnFailure());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("TutorialAppSchema");

            modelBuilder.Entity<User>() // Call Entity
                .ToTable("Users", "TutorialAppSchema") // Set the table and schema details
                .HasKey(u => u.UserId); // Provide the key 
            
            modelBuilder.Entity<UserJobInfo>() 
                .HasKey(u => u.UserId); // Provide the key

            modelBuilder.Entity<UserSalary>() 
                .HasKey(u => u.UserId); // Provide the key
        }

    }
}