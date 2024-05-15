using MassTransit;
using TripService.Data;
using Shared.Flight.Events;

namespace TripService.Consumers
{
    public class ChangeSeatsAvailabilityStatusConsumer : IConsumer<SeatsAvailabilityAfterReservationEvent>
    {
        private readonly ITripRepo _TripRepo;

        public ChangeSeatsAvailabilityStatusConsumer(ITripRepo TripRepo)
        {
            _TripRepo = TripRepo;
        }

        public async Task Consume(ConsumeContext<SeatsAvailabilityAfterReservationEvent> context)
        {
            Guid? flightId = context.Message.FlightId;
            int numOfFreeSeats = context.Message.NumOfFreeSeats;

            var Trips = _TripRepo.GetTripsByFlightId(flightId);
            foreach(var Trip in Trips)
            {
                if ( (Trip.NumOfAdults + Trip.NumOfKidsTo18 + Trip.NumOfKidsTo10) > numOfFreeSeats ) 
                {
                    _TripRepo.ChangeReservationStatus(Trip.Id, "Lack of flight seats", null);
                } 
                else if ( Trip.Status == "Lack of flight seats" )  
                {
                    _TripRepo.ChangeReservationStatus(Trip.Id, "Available", null);
                }
            }
        }
    }
}