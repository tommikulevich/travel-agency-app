using MassTransit;
using FlightService.Repo;
using Shared.Flight.Events;

namespace Flight.Consumers
{
    public class ReserveSeatsEventConsumer : IConsumer<ReserveSeatsEvent>
    {
        private readonly IFlightRepo _flightService;

        public ReserveSeatsEventConsumer(IFlightRepo flightService)
        {
            _flightService = flightService;
        }

        public async Task Consume(ConsumeContext<ReserveSeatsEvent> context)
        {
            try
            {
                _flightService.ReserveSeats(context.Message.FlightId, context.Message.Seats);
                await context.RespondAsync(new ReserveSeatsReplyEvent(
                    context.Message.CorrelationId ,
                    context.Message.FlightId, 
                    true, 
                    "Seats successfully reserved."
                ));

                int numOfFreeSeats = _flightService.GetNumOfFreeSeatsOfSpecificFlight(context.Message.FlightId);
                await context.Publish(new SeatsAvailabilityAfterReservationEvent {
                    Id = Guid.NewGuid(),
                    FlightId = context.Message.FlightId,
                    NumOfFreeSeats = numOfFreeSeats
                });
            }
            catch (Exception ex)
            {
                await context.RespondAsync(new ReserveSeatsReplyEvent(
                    context.Message.CorrelationId ,
                    context.Message.FlightId, 
                    false, 
                    $"Failed to reserve seats: {ex.Message}"
                ));
            }
        }
    }
}
