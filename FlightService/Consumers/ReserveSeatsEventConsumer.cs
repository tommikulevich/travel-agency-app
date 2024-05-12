using System;
using Shared.Flight.Events;
using FlightService.Repo;
using MassTransit;

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
                await context.RespondAsync(new ReserveSeatsReplyEvent(context.Message.FlightId, true, "Seats successfully reserved."));
            }
            catch (Exception ex)
            {
                await context.RespondAsync(new ReserveSeatsReplyEvent(context.Message.FlightId, false, $"Failed to reserve seats: {ex.Message}"));
            }
        }
    }
}
