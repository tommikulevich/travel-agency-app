using Microsoft.EntityFrameworkCore;
using HotelService.Models;

namespace HotelService.Data 
{
    public class HotelDbContext : DbContext 
    {
        public DbSet<Hotel> Hotel { get; set; }
        public DbSet<Room> Room { get; set; }
        public DbSet<RoomEvent> RoomEvent { get; set; }

        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}