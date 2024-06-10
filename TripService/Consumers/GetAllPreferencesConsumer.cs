using MassTransit;
using TripService.Data;
using Shared.Trip.Events;
using Shared.Trip.Dtos;

namespace TripService.Consumers
{
    public class GetAllPreferencesConsumer : IConsumer<GetAllPreferencesEvent>
    {
        private readonly ITripRepo _tripRepo;

        public GetAllPreferencesConsumer(ITripRepo tripRepo)
        {
            _tripRepo = tripRepo;
        }

        public async Task Consume(ConsumeContext<GetAllPreferencesEvent> context)
        {

            string topDestination = _tripRepo.GetMostPopularReservedDestination();
            string topHotel = _tripRepo.GetMostPopularReservedHotel();
            string topRoom = _tripRepo.GetMostPopularReservedRoom();
            string topTransport = _tripRepo.GetMostPopularReservedTransport();
 
            PreferencesDto pref = new PreferencesDto()
            {
                destinationPreference = topDestination,
                hotelPreference = topHotel,
                roomPreference = topRoom,
                transportPreference = topTransport
            };
            await context.RespondAsync<GetAllPreferencesReplyEvent>(
                new GetAllPreferencesReplyEvent() { 
                    CorrelationId = context.Message.CorrelationId, 
                    Preferences = pref
            });
        }
    }
}