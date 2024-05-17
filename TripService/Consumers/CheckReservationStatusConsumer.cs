using MassTransit;
using TripService.Data;
using Shared.Trip.Events;

namespace TripService.Consumers
{
    public class CheckReservationStatusConsumer : IConsumer<CheckReservationStatusEvent>
    {
        private readonly ITripRepo _tripRepo;

        public CheckReservationStatusConsumer(ITripRepo tripRepo)
        {
            _tripRepo = tripRepo;
        }

        public async Task Consume(ConsumeContext<CheckReservationStatusEvent> context)
        {
            Console.WriteLine("Checking reservaion status");
            
            Guid CorrelationId = context.Message.CorrelationId;
            bool isAvailable = _tripRepo.CheckAvailability(CorrelationId);
            await context.RespondAsync<CheckReservationStatusReplyEvent>(
                new CheckReservationStatusReplyEvent() { 
                    CorrelationId = context.Message.CorrelationId, 
                    available = isAvailable
            });
        }
    }
}