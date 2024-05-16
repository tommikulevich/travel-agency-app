using MassTransit;
using TripService.Data;
using Shared.Trip.Events;
using Shared.Trip.Dtos;
using MassTransit.Serialization;

namespace TripService.Consumers
{
    public class GetTripsByPreferencesConsumer : IConsumer<GetTripByPreferencesEvent>
    {
        private readonly ITripRepo _TripRepo;
        public GetTripsByPreferencesConsumer(ITripRepo TripRepo)
        {
            _TripRepo = TripRepo;
        }
        public async Task Consume(ConsumeContext<GetTripByPreferencesEvent> context)
        {
            var Destination = context.Message.Destination;
            var DepartureDate = context.Message.DepartureDate;
            var DeparturePlace = context.Message.DeparturePlace;
            var NumOfAdults = context.Message.NumOfAdults;
            var NumOfKidsTo18 = context.Message.NumOfKidsTo18;
            var NumOfKidsTo10 = context.Message.NumOfKidsTo10;
            var NumOfKidsTo3 = context.Message.NumOfKidsTo3;
            var Trips = _TripRepo.GetTripsByPreferences(Destination, DepartureDate, 
                                                        DeparturePlace, NumOfAdults, 
                                                        NumOfKidsTo18, NumOfKidsTo10, 
                                                        NumOfKidsTo3);
            var TripsDto = new List<TripDto>();
            foreach(var Trip in Trips)
            {
                var TripDto = Trip.ToTripDto();
                TripsDto.Add(TripDto);
            }
            await context.RespondAsync<ReplyTripsDtosEvent>(new ReplyTripsDtosEvent() { CorrelationId = context.Message.CorrelationId, Trips = TripsDto});

        }
    }

}