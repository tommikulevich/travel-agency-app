using MassTransit;
using TripService.Data;
using Shared.Trip.Events;
using Shared.Trip.Dtos;

namespace TripService.Consumers
{
    public class GetTripByUserIdConsumer : IConsumer<GetTripsByUserIdEvent>
    {
        private readonly ITripRepo _TripRepo;
        public GetTripByUserIdConsumer(ITripRepo TripRepo)
        {
            _TripRepo = TripRepo;
        }
        public async Task Consume(ConsumeContext<GetTripsByUserIdEvent> context)
        {
            var clientId = context.Message.ClientId;
            var Trips = _TripRepo.GetTripById(clientId);
            var TripsDto = new List<TripDto>();
            foreach(var Trip in Trips)
            {
                var TripDto = Trip.ToTripDto();
                TripsDto.Add(TripDto);
            }
            await context.RespondAsync<ReplyTripsDtosEvent>(new ReplyTripsDtosEvent() { CorrelationId = context.Message.CorrelationId, Trips = TripsDto});

            Console.WriteLine("Jestem client");

        }
    }

}