using Shared.Hotel.Events;
using HotelService.Models;

namespace HotelService.Data {
    public interface IHotelRepo {
        List<Hotel> GetAvailableHotels(AvailableRoomsRequest request);
    }
}