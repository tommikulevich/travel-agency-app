using MassTransit;
using TripService.Data;
using Shared.Trip.Events;
using Shared.Trip.Dtos;

namespace TripService.Consumers
{
    public class ChangeReservationStatusConsumer : IConsumer<ChangeReservationStatusEvent>
    {
        private readonly ITripRepo _TripRepo;
        public ChangeReservationStatusConsumer(ITripRepo TripRepo)
        {
            _TripRepo = TripRepo;
        }
        public async Task Consume(ConsumeContext<ChangeReservationStatusEvent> context)
        {
            Console.WriteLine("Reservation status changed");
            Guid? ClientId = context.Message.ClientId;
            var offerId = context.Message.OfferId;
            var status = context.Message.Status;
            _TripRepo.ChangeReservationStatus(offerId, status, ClientId);
            // var TripsDto = new List<TripDto>();
            // int i = 0;
            // foreach(var Trip in Trips)
            // {
            //     var TripDto = Trip.ToTripDto();
            //     TripsDto.Add(TripDto);
            //     i++;
            // }
            // await context.RespondAsync<ReplyTripsDtosEvent>(new ReplyTripsDtosEvent() { CorrelationId = context.Message.CorrelationId, Trips = TripsDto});

        }
    }

}