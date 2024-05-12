using Microsoft.EntityFrameworkCore;
using TripService.Models;

namespace TripService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {
            
        }

        public DbSet<Trip> Trip { get; set; }
    }
}