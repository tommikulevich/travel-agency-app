using MassTransit;
using HotelService.Dtos;
using HotelService.Data;

namespace HotelService.Consumers {
    public class AvailableRoomsConsumer : IConsumer<AvailableRoomsRequest> {
        private readonly IHotelRepo _hotelRepo;

        public AvailableRoomsConsumer(IHotelRepo hotelRepo) {
            _hotelRepo = hotelRepo;
        }

        public async Task Consume(ConsumeContext<AvailableRoomsRequest> context) {
            var hotels = _hotelRepo.GetAvailableHotels(context.Message);
            await context.RespondAsync(hotels);
        }
    }
}