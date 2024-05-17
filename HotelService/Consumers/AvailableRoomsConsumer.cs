using MassTransit;
using HotelService.Data;
using Shared.Hotel.Events;

namespace HotelService.Consumers 
{
    public class AvailableRoomsConsumer : IConsumer<AvailableRoomsRequest> 
    {
        private readonly IHotelRepo _hotelRepo;

        public AvailableRoomsConsumer(IHotelRepo hotelRepo) 
        {
            _hotelRepo = hotelRepo;
        }

        public async Task Consume(ConsumeContext<AvailableRoomsRequest> context) 
        {
            var hotels = _hotelRepo.GetAvailableHotels(context.Message);
            await context.RespondAsync(new { Hotels = hotels });
        }
    }
}