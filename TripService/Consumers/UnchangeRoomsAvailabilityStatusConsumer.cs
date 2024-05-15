using MassTransit;
using TripService.Data;
using Shared.Hotel.Events;

namespace TripService.Consumers
{
    public class UnchangeRoomsAvailabilityStatusConsumer : IConsumer<RoomsAvailabilityAfterUnreservationEvent>
    {
        private readonly ITripRepo _TripRepo;

        public UnchangeRoomsAvailabilityStatusConsumer(ITripRepo TripRepo)
        {
            _TripRepo = TripRepo;
        }

        public async Task Consume(ConsumeContext<RoomsAvailabilityAfterUnreservationEvent> context)
        {
            var unreservedTrip = _TripRepo.GetTripByGuid(context.Message.CorrelationId);
            var Trips = _TripRepo.GetTripsBySpecificRoomConfiguration(
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
                    _TripRepo.ChangeReservationStatus(Trip.Id, "Available", null);
                }
            }
        }
    }
}