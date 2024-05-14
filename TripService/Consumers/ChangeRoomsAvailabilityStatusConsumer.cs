using MassTransit;
using TripService.Data;
using Shared.Hotel.Events;

namespace TripService.Consumers
{
    public class ChangeRoomsAvailabilityStatusConsumer : IConsumer<RoomsAvailabilityAfterReservationEvent>
    {
        private readonly ITripRepo _TripRepo;

        public ChangeRoomsAvailabilityStatusConsumer(ITripRepo TripRepo)
        {
            _TripRepo = TripRepo;
        }

        public async Task Consume(ConsumeContext<RoomsAvailabilityAfterReservationEvent> context)
        {
            var Trips = _TripRepo.GetTripsBySpecificRoomConfiguration(
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
                _TripRepo.ChangeReservationStatus(Trip.Id, "Lack of rooms", null);
            }
        }
    }

}