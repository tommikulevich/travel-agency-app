using Microsoft.EntityFrameworkCore;
using OffersServise.Models;

namespace OffersServise.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {
            
        }

        public DbSet<Offer> Offers { get; set; }
    }
}