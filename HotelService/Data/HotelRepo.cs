using Microsoft.EntityFrameworkCore;
using HotelService.Models;
using Shared.Hotel.Events;

namespace HotelService.Data 
{
    public class HotelRepo : IHotelRepo 
    {
        private readonly HotelDbContext _context;

        public HotelRepo(HotelDbContext context) 
        {
            _context = context;
        }

        public List<Hotel> GetAvailableHotels(AvailableRoomsRequest request) 
        {
            return _context.Hotel.Include(h => h.Rooms)
                .ThenInclude(r => r.RoomEvents)
                .Where(h => h.Rooms.Any(r => 
                    r.RoomEvents.All(
                        re => re.Status != "Reserved" 
                        || re.EndDate <= request.DepartureTime 
                        || re.StartDate >= request.ArrivalTime)))
                .ToList();
        }
    }
}