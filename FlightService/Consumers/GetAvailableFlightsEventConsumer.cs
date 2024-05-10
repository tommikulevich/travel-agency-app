using System;
using Shared.Flight.Events;
using Shared.Flight.Dtos;
using FlightService.Repo;
using MassTransit;
using FlightService.Data.Tables;
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

            await context.RespondAsync(new GetAvailableFlightsEventReply(flightsDto));
        }
    }
}
// await context.RespondAsync<ReplyTripsDtosEvent>(new ReplyTripsDtosEvent() { CorrelationId = context.Message.CorrelationId, Trips = TripsDto});
//  var clientId = context.Message.ClientId;
//             var Trips = _TripRepo.GetTripById(clientId);
//             var TripsDto = new List<TripDto>();
//             foreach(var Trip in Trips)
//             {
//                 var TripDto = Trip.ToTripDto();
//                 TripsDto.Add(TripDto);
//             }