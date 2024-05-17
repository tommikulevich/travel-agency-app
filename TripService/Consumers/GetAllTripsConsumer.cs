using MassTransit;
using TripService.Data;
using Shared.Trip.Events;
using Shared.Trip.Dtos;

namespace TripService.Consumers
{
    public class GetAllTripsConsumer : IConsumer<GetAllTripsEvent>
    {
        private readonly ITripRepo _tripRepo;

        public GetAllTripsConsumer(ITripRepo tripRepo)
        {
            _tripRepo = tripRepo;
        }

        public async Task Consume(ConsumeContext<GetAllTripsEvent> context)
        {
            var Trips = _tripRepo.GetAllTrips();
            var TripsDto = new List<TripDto>();
            foreach(var Trip in Trips)
            {
                var TripDto = Trip.ToTripDto();
                TripsDto.Add(TripDto);
            }

            await context.RespondAsync<ReplyTripsDtosEvent>(
                new ReplyTripsDtosEvent() { 
                    CorrelationId = context.Message.CorrelationId, 
                    Trips = TripsDto
            });
        }
    }
}