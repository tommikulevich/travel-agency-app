using Microsoft.EntityFrameworkCore;
using ApiGateway.Models;

namespace ApiGateway.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> opt) : base(opt)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().HasKey(u => u.UserName);
            modelBuilder.Entity<User>().HasKey(u => u.Password);
            // Add other entity configurations if needed
        }

        public DbSet<User> User { get; set; }
    }
}