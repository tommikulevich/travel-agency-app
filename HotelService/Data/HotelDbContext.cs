using HotelService.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelService.Data {
    public class HotelDbContext : DbContext {
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomEvent> RoomEvents { get; set; }

        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
        }
    }
}