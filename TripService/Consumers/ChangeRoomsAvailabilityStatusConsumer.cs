using MassTransit;
using TripService.Data;
using Shared.Hotel.Events;

namespace TripService.Consumers
{
    public class ChangeRoomsAvailabilityStatusConsumer : IConsumer<RoomsAvailabilityAfterReservationEvent>
    {
        private readonly ITripRepo _tripRepo;

        public ChangeRoomsAvailabilityStatusConsumer(ITripRepo tripRepo)
        {
            _tripRepo = tripRepo;
        }

        public async Task Consume(ConsumeContext<RoomsAvailabilityAfterReservationEvent> context)
        {
            var Trips = _tripRepo.GetTripsBySpecificRoomConfiguration(
                context.Message.HotelId,
                context.Message.NumOfAdults,
                context.Message.NumOfKidsTo18,
                context.Message.NumOfKidsTo10,
                context.Message.NumOfKidsTo3,
                context.Message.ArrivalDate,
                context.Message.ReturnDate,
                context.Message.RoomType
            );

            foreach(var Trip in Trips)
            {
                _tripRepo.ChangeReservationStatus(Trip.Id, "Brak pokoi", null);
            }

            await Task.Yield();     // Ensures that method runs asynchronously and avoids the warning
        }
    }

}