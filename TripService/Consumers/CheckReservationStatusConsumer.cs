using MassTransit;
using TripService.Data;
using Shared.Trip.Events;
using Shared.Trip.Dtos;

namespace TripService.Consumers
{
    public class CheckReservationStatusConsumer : IConsumer<CheckReservationStatusEvent>
    {
        private readonly ITripRepo _TripRepo;
        public CheckReservationStatusConsumer(ITripRepo TripRepo)
        {
            _TripRepo = TripRepo;
        }
        public async Task Consume(ConsumeContext<CheckReservationStatusEvent> context)
        {
            Console.WriteLine("Checking reservaion status");
            Guid CorrelationId = context.Message.CorrelationId;
            bool isAvailable = _TripRepo.CheckAvailability(CorrelationId);
            await context.RespondAsync<CheckReservationStatusReplyEvent>(new CheckReservationStatusReplyEvent() { CorrelationId = context.Message.CorrelationId, available=isAvailable});

        }
    }

}