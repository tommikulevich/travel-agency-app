using Shared.Hotel.Events;
using HotelService.Models;

namespace HotelService.Data {
    public class HotelRepo : IHotelRepo {
        private readonly HotelDbContext _context;

        public HotelRepo(HotelDbContext context) {
            _context = context;
        }

        public List<Hotel> GetAvailableHotels(AvailableRoomsRequest request) {
            return _context.Hotels.ToList();
        }
    }
}