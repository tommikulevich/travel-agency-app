using MassTransit;
using TripService.Data;
using Shared.Trip.Events;

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
            
            _tripRepo.ChangeReservationStatus(offerId, status, ClientId);

            await Task.Yield();     // Ensures that method runs asynchronously and avoids the warning
        }
    }
}