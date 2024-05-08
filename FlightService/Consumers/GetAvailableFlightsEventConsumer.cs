using System;
using FlightService.Events;
using FlightService.Repo;
using MassTransit;

namespace Flight.Consumers
{
    public class GetAvailableFlightsEventConsumer : IConsumer<GetAvailableFlightsEvent>
    {
        private readonly IFlightRepo _flightService;

        public GetAvailableFlightsEventConsumer(IFlightRepo flightService)
        {
            _flightService = flightService;
        }

        public async Task Consume(ConsumeContext<GetAvailableFlightsEvent> context)
        {
            var flights = _flightService.GetAvailableFlights(
                context.Message.DeparturePlace,
                context.Message.ArrivalPlace,
                context.Message.DepartureTime,
                context.Message.ArrivalTime,
                context.Message.FreeSeats
            );

            await context.RespondAsync(new GetAvailableFlightsEventReply(flights.ToList()));
        }
    }
}
