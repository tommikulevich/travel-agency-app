using MassTransit;
using FlightService.Repo;
using Shared.Flight.Events;

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

            var flightsDto = new List<Shared.Flight.Dtos.FlightDto>();
            foreach(var flight in flights)
            {
                var flightDto = flight.ToFlightDto();
                flightsDto.Add(flightDto);
            }

            await context.RespondAsync(new GetAvailableFlightsReplyEvent(flightsDto));
        }
    }
}