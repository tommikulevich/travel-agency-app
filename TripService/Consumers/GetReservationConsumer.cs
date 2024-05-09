using MassTransit;
using TripService.Data;
using Shared.Trip.Events;
using Shared.Trip.Dtos;

namespace TripService.Consumers
{
    public class GetTripConsumer : IConsumer<GetTripEvent>
    {
        private readonly ITripRepo _TripRepo;
        public GetTripConsumer(ITripRepo TripRepo)
        {
            _TripRepo = TripRepo;
        }
        public async Task Consume(ConsumeContext<GetTripEvent> context)
        {
            var userId = context.Message.UserId;
            var Trips = _TripRepo.GetAllTrips();
            var TripsDto = new List<TripDto>();
            foreach(var Trip in Trips)
            {
                var TripDto = Trip.ToTripDto();
                TripsDto.Add(TripDto);
            }
            await context.RespondAsync<GetTripReplyEvent>(new GetTripReplyEvent() { CorrelationId = context.Message.CorrelationId, Trips = TripsDto});
            // Fetch reservations from the database
            Console.WriteLine("Jestem");

        }
    }

}