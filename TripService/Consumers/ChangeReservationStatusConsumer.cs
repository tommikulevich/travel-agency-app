using MassTransit;
using TripService.Data;
using Shared.Trip.Events;
using MassTransit.SqlTransport.Topology;
using Shared.ApiGateway.Events;

namespace TripService.Consumers
{
    public class ChangeReservationStatusConsumer : IConsumer<ChangeReservationStatusEvent>
    {
        private readonly ITripRepo _tripRepo;
        
        public ChangeReservationStatusConsumer(ITripRepo tripRepo)
        {
            _tripRepo = tripRepo;
        }

        public async Task Consume(ConsumeContext<ChangeReservationStatusEvent> context)
        {
            Console.WriteLine("Reservation status changed");
            
            Guid? ClientId = context.Message.ClientId;
            var offerId = context.Message.OfferId;
            var status = context.Message.Status;
            string previous_top = "";
            string new_top = "";

            if (status == "Zarezerwowana")
            {
                previous_top = _tripRepo.GetMostPopularReservedDestination();
            }
            
            
            _tripRepo.ChangeReservationStatus(offerId, status, ClientId);

            if (status == "Zarezerwowana")
            {
                new_top = _tripRepo.GetMostPopularReservedDestination();

                if (new_top != previous_top)
                {
                    await context.Publish(new NewDestinationPreferenceEvent() {
                    CorrelationId = Guid.NewGuid(), 
                    newPreference = new_top
                    });
                    Console.WriteLine("Preference changed");
                    Console.WriteLine(new_top);
                    Console.WriteLine(previous_top);
                }
                else
                {
                    Console.WriteLine("Preference not changed");
                    Console.WriteLine(new_top);
                    Console.WriteLine(previous_top);
                }
            }

            await Task.Yield();     // Ensures that method runs asynchronously and avoids the warning
        }
    }
}