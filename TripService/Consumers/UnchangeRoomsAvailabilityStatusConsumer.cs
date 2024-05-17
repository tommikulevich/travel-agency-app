using MassTransit;
using TripService.Data;
using Shared.Hotel.Events;

namespace TripService.Consumers
{
    public class UnchangeRoomsAvailabilityStatusConsumer : IConsumer<RoomsAvailabilityAfterUnreservationEvent>
    {
        private readonly ITripRepo _tripRepo;

        public UnchangeRoomsAvailabilityStatusConsumer(ITripRepo tripRepo)
        {
            _tripRepo = tripRepo;
        }

        public async Task Consume(ConsumeContext<RoomsAvailabilityAfterUnreservationEvent> context)
        {            
            var unreservedTrip = _tripRepo.GetTripByGuid(context.Message.CorrelationId);
            var Trips = _tripRepo.GetTripsBySpecificRoomConfiguration(
                unreservedTrip.HotelId,
                unreservedTrip.NumOfAdults,
                unreservedTrip.NumOfKidsTo18,
                unreservedTrip.NumOfKidsTo10,
                unreservedTrip.NumOfKidsTo3,
                unreservedTrip.DepartureDate,
                unreservedTrip.ReturnDate,
                unreservedTrip.RoomType
            );

            foreach(var Trip in Trips)
            {
                if ( Trip.Status == "Lack of flight seats" ) 
                {
                    _tripRepo.ChangeReservationStatus(Trip.Id, "Available", null);
                }
            }

            await Task.Yield();     // Ensures that method runs asynchronously and avoids the warning
        }
    }
}