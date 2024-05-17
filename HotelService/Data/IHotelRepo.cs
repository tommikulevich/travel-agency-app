using HotelService.Models;
using Shared.Hotel.Events;

namespace HotelService.Data 
{
    public interface IHotelRepo 
    {
        List<Hotel> GetAvailableHotels(AvailableRoomsRequest request);
    }
}