using MassTransit;
using TripService.Data;
using Shared.Flight.Events;

namespace TripService.Consumers
{
    public class ChangeSeatsAvailabilityStatusConsumer : IConsumer<SeatsAvailabilityAfterReservationEvent>
    {
        private readonly ITripRepo _tripRepo;

        public ChangeSeatsAvailabilityStatusConsumer(ITripRepo tripRepo)
        {
            _tripRepo = tripRepo;
        }

        public async Task Consume(ConsumeContext<SeatsAvailabilityAfterReservationEvent> context)
        {
            Guid? flightId = context.Message.FlightId;
            int numOfFreeSeats = context.Message.NumOfFreeSeats;

            var Trips = _tripRepo.GetTripsByFlightId(flightId);
            foreach(var Trip in Trips)
            {
                if ( (Trip.NumOfAdults + Trip.NumOfKidsTo18 + Trip.NumOfKidsTo10) > numOfFreeSeats ) 
                {
                    _tripRepo.ChangeReservationStatus(Trip.Id, "Brak miejsc w samolocie", null);
                } 
                else if ( Trip.Status == "Brak miejsc w samolocie" )  
                {
                    _tripRepo.ChangeReservationStatus(Trip.Id, "Dostępna", null);
                }
            }

            await Task.Yield();     // Ensures that method runs asynchronously and avoids the warning
        }
    }
}