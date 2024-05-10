using MassTransit;
using TripService.Data;
using Shared.Trip.Events;
using Shared.Trip.Dtos;

namespace TripService.Consumers
{
    public class GetAllTripsConsumer : IConsumer<GetAllTripsEvent>
    {
        private readonly ITripRepo _TripRepo;
        public GetAllTripsConsumer(ITripRepo TripRepo)
        {
            _TripRepo = TripRepo;
        }
        public async Task Consume(ConsumeContext<GetAllTripsEvent> context)
        {
            var Trips = _TripRepo.GetAllTrips();
            var TripsDto = new List<TripDto>();
            int i = 0;
            foreach(var Trip in Trips)
            {
                var TripDto = Trip.ToTripDto();
                TripsDto.Add(TripDto);
                i++;
            }
            await context.RespondAsync<ReplyTripsDtosEvent>(new ReplyTripsDtosEvent() { CorrelationId = context.Message.CorrelationId, Trips = TripsDto});
            // Fetch reservations from the database
            Console.WriteLine("Jestem all", i);

        }
    }

}