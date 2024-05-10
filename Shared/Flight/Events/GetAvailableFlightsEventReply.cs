using System;
using System.Collections.Generic;
using Shared.Flight.Dtos;

namespace Shared.Flight.Events
{
    public class GetAvailableFlightsEventReply
    {
        public Guid CorrelationId  { get; set; }
        public List<FlightDto> AvailableFlights { get; set; }

        public GetAvailableFlightsEventReply(List<FlightDto> availableFlights)
        {
            AvailableFlights = availableFlights;
        }
    }
}
